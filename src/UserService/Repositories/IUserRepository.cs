using UserService.DTOs;
using UserService.Models;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);

 
}
