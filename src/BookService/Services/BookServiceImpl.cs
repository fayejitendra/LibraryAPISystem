using BookService.Models;
using BookService.Repositories;

namespace BookService.Services;

public class BookServiceImpl : IBookService
{
    private readonly IBookRepository _repo;
    public BookServiceImpl(IBookRepository repo) { _repo = repo; }

    public async Task<List<Book>> GetAllBooksAsync() => await _repo.GetAllAsync();

    public async Task<Book?> GetBookByIdAsync(int id) => await _repo.GetByIdAsync(id);
}
