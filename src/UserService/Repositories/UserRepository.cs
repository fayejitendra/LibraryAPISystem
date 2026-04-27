using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _db;
    public UserRepository(UserDbContext db) { _db = db; }

    public async Task<User?> GetByIdAsync(int id)
        => await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

}
