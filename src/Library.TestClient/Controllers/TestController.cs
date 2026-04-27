using Microsoft.AspNetCore.Mvc;
using Library.Protos.Book;
using Library.Protos.Analytics;

namespace Library.TestClient.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly BookService.BookServiceClient _bookClient;
    private readonly LibraryAnalytics.LibraryAnalyticsClient _analyticsClient;

    public TestController(
        BookService.BookServiceClient bookClient,
        LibraryAnalytics.LibraryAnalyticsClient analyticsClient)
    {
        _bookClient = bookClient;
        _analyticsClient = analyticsClient;
    }

    // -----------------------------
    //  Get Book
    // -----------------------------
    [HttpGet("book/{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _bookClient.GetBookAsync(new Protos.Book.BookIdRequest { BookId = id });
        return Ok(book);
    }

    // -----------------------------
    // Most Borrowed Books
    // -----------------------------
    [HttpGet("most-borrowed")]
    public async Task<IActionResult> GetMostBorrowed([FromQuery] int top = 5)
    {
        var response = await _analyticsClient.GetMostBorrowedBooksAsync(new TopRequest { Top = top });
        return Ok(response.Books);
    }

    // -----------------------------
    // Top Borrowing Users
    // -----------------------------
    [HttpGet("top-users")]
    public async Task<IActionResult> GetTopUsers([FromQuery] int top = 5, string startDate = "", string endDate = "")
    {
        var response = await _analyticsClient.GetTopBorrowingUsersAsync(new UserActivityRequest
        {
            Top = top,
            StartDate = startDate,
            EndDate = endDate
        });

        return Ok(response.Users);
    }

    // -----------------------------
    // User Reading Pace
    // -----------------------------
    [HttpGet("reading-pace/{userId}")]
    public async Task<IActionResult> GetReadingPace(int userId)
    {
        var response = await _analyticsClient.GetUserReadingPaceAsync(new UserIdRequest { UserId = userId });
        return Ok(response);
    }

    // -----------------------------
    // Also Borrowed Books
    // -----------------------------
    [HttpGet("also-borrowed/{bookId}")]
    public async Task<IActionResult> GetAlsoBorrowed(int bookId)
    {
        var response = await _analyticsClient.GetAlsoBorrowedBooksAsync(new Protos.Analytics.BookIdRequest { BookId = bookId });
        return Ok(response.Books);
    }
}
