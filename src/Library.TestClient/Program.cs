using Library.Protos.Analytics;
using Library.Protos.Book;

var builder = WebApplication.CreateBuilder(args);

// Add controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register gRPC clients
builder.Services.AddGrpcClient<BookService.BookServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:53320"); // BookService server
});
builder.Services.AddGrpcClient<LibraryAnalytics.LibraryAnalyticsClient>(o =>
{
    o.Address = new Uri("https://localhost:53324"); // LendingService server
});
builder.Services.AddGrpcClient<Library.Protos.User.UserService.UserServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:53322"); // UserService server
});

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
