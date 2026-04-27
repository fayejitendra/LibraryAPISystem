using BookService.Models;
using BookService.Repositories;
using BookService.Services;
using Moq;

namespace BookService.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public async Task GetBookAsync_ReturnsBook_WhenExists()
        {
            // Arrange
            var repo = new Mock<IBookRepository>();
            repo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Book { Id = 1, Title = "Test Book" });

            var service = new BookServiceImpl(repo.Object);

            // Act
            var result = await service.GetBookByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Book", result.Title);
        }
    }

}