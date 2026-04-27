using LendingService.Data;
using LendingService.Models;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Repositories
{
    public class LendingRepository : ILendingRepository
    {
        private readonly LendingDbContext _context;

        public LendingRepository(LendingDbContext context)
        {
            _context = context;
        }

        public async Task<LendingRecord> BorrowAsync(int userId, int bookId)
        {
            var record = new LendingRecord
            {
                UserId = userId,
                BookId = bookId,
                BorrowedAt = DateTime.UtcNow,
                ReturnedAt = null
            };

            _context.LendingRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<bool> ReturnAsync(int userId, int bookId)
        {
            var record = await _context.LendingRecords
                .FirstOrDefaultAsync(r =>
                    r.UserId == userId &&
                    r.BookId == bookId &&
                    r.ReturnedAt == null);

            if (record == null) return false;

            record.ReturnedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<LendingRecord>> GetAllAsync()
        {
            return await _context.LendingRecords.ToListAsync();
        }

        public async Task<IEnumerable<LendingRecord>> GetRecordsByBookIdAsync(int bookId)
        {
            return await _context.LendingRecords
                .Where(r => r.BookId == bookId)
                .ToListAsync();
        }

        public async Task<IEnumerable<LendingRecord>> GetRecordsByUserIdAsync(int userId)
        {
            return await _context.LendingRecords
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
