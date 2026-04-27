using Grpc.Core;
using BookService.Services;
using Library.Protos.Book;
using static Library.Protos.Book.BookService;



namespace BookService;

public class BookGrpcService : BookServiceBase
{
    private readonly IBookService _bookService;
    public BookGrpcService(IBookService bookService)
    {
        _bookService = bookService;
    }

    public override async Task<Book> GetBook(BookIdRequest request, ServerCallContext context)
    {
        var book = await _bookService.GetBookByIdAsync(request.BookId);
        return new Book
        {
            Id = book.Id,
            Isbn = book.ISBN,
            Title = book.Title,
            Author = book.Author,
            Pages = book.Pages
        };
    }
}