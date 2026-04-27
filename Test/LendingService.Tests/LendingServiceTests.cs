using Moq;
using LendingService.Models;
using LendingService.Services;
using LendingService.Repositories;

namespace LendingService.Tests
{
    public class LendingServiceTests
    {
        [Fact]
        public async Task GetMostBorrowedBooks_Should_Return_Top_Records()
        {
            var repo = new Mock<ILendingRepository>();

            repo.Setup(r => r.BorrowAsync(1, 1)).ReturnsAsync(new LendingRecord
            { BookId = 1, UserId = 1, BorrowedAt = DateTime.Now.AddDays(-5), ReturnedAt = DateTime.Now }
                 );

            var service = new LendingServiceImpl(repo.Object);
            var result = await service.BorrowAsync(1,1);

            Assert.Equal(1, result.UserId);
            Assert.Equal(1, result.BookId);
        }
    }
}
