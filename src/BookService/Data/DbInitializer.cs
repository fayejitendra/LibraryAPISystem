using BookService.Models;

namespace BookService.Data;
public static class DbInitializer
{
    public static void Initialize(BookDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Books.Any()) return;

        var books = new List<Book>
        {
            new Book {ISBN = "978-0001", Title = "C# in Depth", Author = "Jon Skeet", Pages = 500 },
            new Book { ISBN = "978-0002", Title = "Clean Code", Author = "Robert C. Martin", Pages = 450 },
            new Book { ISBN = "978-0003", Title = "Design Patterns", Author = "Gang of Four", Pages = 395 }
        };

        context.Books.AddRange(books);
        context.SaveChanges();
    }
}
