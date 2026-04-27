using UserService.Models;

namespace UserService.Data;
public static class DbInitializer
{
    public static void Initialize(UserDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any()) return;

        var users = new List<User>
        {
            new User { Name = "Alice", Email = "alice@example.com" },
            new User { Name = "Bob", Email = "bob@example.com" },
            new User {Name = "Charlie", Email = "charlie@example.com" }
        };
        context.Users.AddRange(users);
        context.SaveChanges();
    }
}
