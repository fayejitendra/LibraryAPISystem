using UserService.Models;
using UserService.Repositories;

namespace UserService.Services;

public class UserServiceImpl : IUserService
{
    private readonly IUserRepository _repo;

    public UserServiceImpl(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<User?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);
}
