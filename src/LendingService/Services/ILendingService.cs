using LendingService.Models;
namespace LendingService.Services;
public interface ILendingService
{
    Task<LendingRecord> BorrowAsync(int userId, int bookId);
    Task<bool> ReturnAsync(int userId, int bookId);
}
