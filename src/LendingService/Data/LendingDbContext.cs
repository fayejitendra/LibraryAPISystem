using Microsoft.EntityFrameworkCore;
using LendingService.Models;

namespace LendingService.Data;
public class LendingDbContext : DbContext
{
    public LendingDbContext(DbContextOptions<LendingDbContext> options) : base(options) { }
    public DbSet<LendingRecord> LendingRecords => Set<LendingRecord>();
}
