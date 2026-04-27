using Microsoft.EntityFrameworkCore;
using BookService.Data;
using BookService.Models;

namespace BookService.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookDbContext _db;
    public BookRepository(BookDbContext db) { _db = db; }

    public async Task<List<Book>> GetAllAsync() => await _db.Books.ToListAsync();

    public async Task<Book?> GetByIdAsync(int id) => await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
}
