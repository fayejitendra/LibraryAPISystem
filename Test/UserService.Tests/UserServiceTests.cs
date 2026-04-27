using Moq;
using UserService.Models;
using UserService.Services;

namespace UserService.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserAsync_ReturnsUser_WhenExists()
        {
            // Arrange
            var repo = new Mock<IUserRepository>();
            repo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User { Id = 1, Name = "Test User" , Email = "Test Email"});

            var service = new UserServiceImpl(repo.Object);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
        }
    }
}