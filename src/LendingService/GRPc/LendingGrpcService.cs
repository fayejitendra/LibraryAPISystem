using Grpc.Core;
using LendingService.Repositories;
using Library.Protos.Analytics; // LibraryAnalyticsBase
using Library.Protos.Book;      // BookServiceClient
using Library.Protos.User;
using TopUsersResponse = Library.Protos.Analytics.TopUsersResponse;
using UserActivityRequest = Library.Protos.Analytics.UserActivityRequest;
using UserIdRequest = Library.Protos.Analytics.UserIdRequest; // Analytics


namespace LendingService.Grpc
{
    public class LendingGrpcService : LibraryAnalytics.LibraryAnalyticsBase
    {
        private readonly ILendingRepository _lendingRepo;
        private readonly BookService.BookServiceClient _bookClient;
        private readonly UserService.UserServiceClient _userClient;

        public LendingGrpcService(
            ILendingRepository lendingRepo,
            BookService.BookServiceClient bookClient, UserService.UserServiceClient userClient)   // From book.proto
        {
            _lendingRepo = lendingRepo;
            _bookClient = bookClient;
            _userClient = userClient;
        }


        // -----------------------------
        // GetMostBorrowedBooks
        // -----------------------------
        public override async Task<MostBorrowedBooksResponse> GetMostBorrowedBooks(
            TopRequest request, ServerCallContext context)
        {
            try
            {
                var lendings = await _lendingRepo.GetAllAsync();

                // Compute borrow counts
                var borrowCounts = lendings
                    .GroupBy(l => l.BookId)
                    .Select(g => new Library.Protos.Analytics.BookBorrowStats
                    {
                        BookId = g.Key,
                        Title = "", // We'll fetch via BookService
                        BorrowCount = g.Count()
                    })
                    .OrderByDescending(b => b.BorrowCount)
                    .Take(request.Top);

                var response = new MostBorrowedBooksResponse();

                foreach (var stat in borrowCounts)
                {
                    // Fetch title from Book Service
                    var book = await _bookClient.GetBookAsync(new Library.Protos.Book.BookIdRequest { BookId = stat.BookId });
                    stat.Title = book?.Title ?? "Unknown";

                    response.Books.Add(stat);
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetMostBorrowedBooks error: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }

        }


        // -----------------------------
        // Estimate User Reading Pace
        // -----------------------------
        public override async Task<ReadingPaceResponse> GetUserReadingPace(UserIdRequest request, ServerCallContext context)
        {
            try
            {


                var lendings = (await _lendingRepo.GetRecordsByUserIdAsync(request.UserId))
                    .Where(l => l.ReturnedAt.HasValue)
                    .ToList();

                if (!lendings.Any())
                    return new ReadingPaceResponse
                    {
                        UserId = request.UserId,
                        PagesPerDay = 0,
                        EstimatedSpeedCategory = "No Data"
                    };

                double totalPages = 0;
                double totalDays = 0;

                foreach (var lend in lendings)
                {
                    // Call BookService via gRPC client
                    var book = await _bookClient.GetBookAsync(
                        new Library.Protos.Book.BookIdRequest { BookId = lend.BookId }
                    );

                    if (book == null) continue;

                    double days = (lend.ReturnedAt.Value - lend.BorrowedAt).TotalDays;
                    if (days <= 0) continue;

                    totalPages += book.Pages;
                    totalDays += days;
                }

                double pagesPerDay = totalDays > 0 ? totalPages / totalDays : 0;

                string speedCategory =
                    pagesPerDay > 50 ? "Fast" :
                    pagesPerDay > 20 ? "Average" :
                    pagesPerDay > 5 ? "Slow" :
                    "Very Slow";

                return new ReadingPaceResponse
                {
                    UserId = request.UserId,
                    PagesPerDay = pagesPerDay,
                    EstimatedSpeedCategory = speedCategory
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserReadingPace error: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }

        // -----------------------------
        // Get Also Borrowed Books
        // -----------------------------
        public override async Task<AlsoBorrowedResponse> GetAlsoBorrowedBooks(Library.Protos.Analytics.BookIdRequest request, ServerCallContext context)
        {
            try
            {


                // Users who borrowed the given book
                var users = (await _lendingRepo.GetRecordsByBookIdAsync(request.BookId))
                    .Select(l => l.UserId)
                    .Distinct()
                    .ToList();

                if (!users.Any()) return new AlsoBorrowedResponse();

                // Other books borrowed by these users
                var otherBookIds = (await _lendingRepo.GetAllAsync())
                    .Where(l => users.Contains(l.UserId) && l.BookId != request.BookId)
                    .Select(l => l.BookId)
                    .Distinct()
                    .ToList();

                var response = new AlsoBorrowedResponse();

                foreach (var id in otherBookIds)
                {
                    var book = await _bookClient.GetBookAsync(
                        new Library.Protos.Book.BookIdRequest { BookId = id }
                    );

                    if (book != null)
                    {
                        response.Books.Add(new BookInfo
                        {
                            BookId = book.Id,
                            Title = book.Title
                        });
                    }
                }

                return response;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"GetAlsoBorrowedBooks error: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }

        // -----------------------------
        // GetTopBorrowingUsers
        // -----------------------------
        public override async Task<TopUsersResponse> GetTopBorrowingUsers(UserActivityRequest request, ServerCallContext context)
        {
            try
            {


                var lendings = await _lendingRepo.GetAllAsync();

                var start = DateTime.Parse(request.StartDate);
                var end = DateTime.Parse(request.EndDate);

                var topUsers = lendings
                    .Where(l => l.BorrowedAt >= start && l.BorrowedAt <= end)
                    .GroupBy(l => l.UserId)
                    .Select(g => new { UserId = g.Key, BorrowCount = g.Count() })
                    .OrderByDescending(x => x.BorrowCount)
                    .Take(request.Top)
                    .ToList();

                var response = new TopUsersResponse();

                foreach (var u in topUsers)
                {
                    try
                    {
                        var user = await _userClient.GetByIdAsync(new Library.Protos.User.UserIdRequest { UserId = u.UserId });
                        response.Users.Add(new Library.Protos.Analytics.UserBorrowStats
                        {
                            UserId = u.UserId,
                            Name = user.Name,
                            BorrowCount = u.BorrowCount
                        });
                    }
                    catch
                    {
                        response.Users.Add(new Library.Protos.Analytics.UserBorrowStats
                        {
                            UserId = u.UserId,
                            Name = "Unknown",
                            BorrowCount = u.BorrowCount
                        });
                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTopBorrowingUsers error: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }
    }
}
