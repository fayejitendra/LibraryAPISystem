using LendingService.Models;

namespace LendingService.Data;
public static class DbInitializer
{
    public static void Initialize(LendingDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.LendingRecords.Any()) return;

        var records = new List<LendingRecord>
        {
            new LendingRecord { UserId = 1, BookId = 1, BorrowedAt = DateTime.UtcNow.AddDays(-10), ReturnedAt = DateTime.UtcNow.AddDays(-5) },
            new LendingRecord { UserId = 1, BookId = 2, BorrowedAt = DateTime.UtcNow.AddDays(-7), ReturnedAt = DateTime.UtcNow.AddDays(-2) },
            new LendingRecord { UserId = 2, BookId = 1, BorrowedAt = DateTime.UtcNow.AddDays(-3) }
        };

        context.LendingRecords.AddRange(records);
        context.SaveChanges();
    }
}
