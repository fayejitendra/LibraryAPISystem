using LendingService.Models;
namespace LendingService.Repositories;
public interface ILendingRepository
{
    Task<LendingRecord> BorrowAsync(int userId, int bookId);
    Task<bool> ReturnAsync(int userId, int bookId);


    Task<IEnumerable<LendingRecord>> GetAllAsync();


    Task<IEnumerable<LendingRecord>> GetRecordsByBookIdAsync(int bookId);

    Task<IEnumerable<LendingRecord>> GetRecordsByUserIdAsync(int userId);
}
