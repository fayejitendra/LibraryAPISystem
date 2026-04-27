# Library Microservices - Sample Implementation (C#, .NET 7)

This repository is a **complete microservices skeleton** for the Library API System described in your problem statement.
It includes three microservices:
- **BookService** — manages books and inventory.
- **UserService** — manages borrowers.
- **LendingService** — manages lending activity (borrows/returns) and reports.

Key features:
- Clean architecture (separation into API layer, service layer, repositories).
- Communication between services via **gRPC** (proto definitions provided).
- **EF Core** with SQL Server (DbContexts and migrations scaffolded).
- Logging with **Serilog**.
- Unit / Integration test project skeletons using **xUnit**.

## How to use

Flow Description

1.	TestClient calls LendingService via gRPC:
o	Requests most borrowed books, top users, reading pace, etc.

2.	LendingService acts as aggregator:
o	Calls BookService to fetch book titles and details (GetBookAsync)
o	Calls UserService to fetch top borrowers (GetTopBorrowingUsersAsync)
o	Computes analytics based on lending repository

3.	BookService provides book details:
o	GetBook by BookId

4.	UserService provides user details:
o	GetUserById and GetTopBorrowingUsers

5.	LendingService compiles results and sends response back to TestClient.

Steps to run the service to local server:

1.	Clone the Repository from blow URL to your local machine– 
https://github.com/fayejitendra/LibraryAPISystem

2.	Specify the connection string for Book, Lending and User service in appsetting.json file 
"ConnectionStrings": {
    "DefaultConnection": "your connection string"
},

Note- As migration script is already included in the solution so it will feed database with sample data.

3.	After this, build the service locally. It should build without any error.
4.	Set Solution setup to run all the services at once so functionality can be tested locally
5.	Run the service, Test client service will land you to swagger page with list of methods

THanks
