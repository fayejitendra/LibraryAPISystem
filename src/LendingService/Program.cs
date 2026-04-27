using LendingService.Data;
using LendingService.Grpc;
using LendingService.Repositories;
using LendingService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<LendingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddGrpcClient<Library.Protos.Book.BookService.BookServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:53320");  // BookService server URL
});

builder.Services.AddGrpcClient<Library.Protos.User.UserService.UserServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:53322");  // BookService server URL
});

builder.Services.AddScoped<ILendingRepository, LendingRepository>();
builder.Services.AddScoped<ILendingService, LendingServiceImpl>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LendingDbContext>();
    DbInitializer.Initialize(dbContext);
}

app.MapGrpcService<LendingGrpcService>();
app.Run();
