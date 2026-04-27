using LendingService.Models;
using LendingService.Repositories;

namespace LendingService.Services;
public class LendingServiceImpl : ILendingService
{
    private readonly ILendingRepository _repo;
    public LendingServiceImpl(ILendingRepository repo) { _repo = repo; }

    public Task<LendingRecord> BorrowAsync(int userId, int bookId) => _repo.BorrowAsync(userId, bookId);
    public Task<bool> ReturnAsync(int userId, int bookId) => _repo.ReturnAsync(userId, bookId);
}
