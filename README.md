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
- Telemetry hooks prepared for **OpenTelemetry**.
- Authentication integration placeholders for **Keycloak** (Docker compose includes a Keycloak instance).
- Docker / Docker Compose to run SQL Server, Keycloak, and the services.
- Unit / Integration test project skeletons using **xUnit**.
- README contains step-by-step instructions to run locally and in Docker.

> ⚠️ Note: This is a compact, runnable skeleton with fully implemented core flows and placeholders where environment-specific secrets are required (database connection string, Keycloak realm/client configuration). You will need `.NET SDK 7` or later and Docker installed.

## How to use

### 1. Unzip & open
Unzip `library-microservices.zip` and open the solution folder.

### 2. Update environment values
Edit `docker-compose.yml` and each service's `appsettings.json` or environment variables to set SQL Server passwords and Keycloak client IDs if you change defaults.

### 3. Run with Docker Compose (recommended)
```bash
docker-compose up --build
```
This will start:
- SQL Server (mssql)
- Keycloak
- BookService, UserService, LendingService (built as Docker images)

Run migrations (one-time, from each service directory):
```bash
dotnet ef database update --project ./src/BookService/BookService.csproj
dotnet ef database update --project ./src/UserService/UserService.csproj
dotnet ef database update --project ./src/LendingService/LendingService.csproj
```

### 4. Run locally (without Docker)
From each service folder:
```bash
dotnet restore
dotnet build
dotnet run
```

### 5. Proto / gRPC
Proto files are under `src/Protos/library.proto`. Services expose gRPC endpoints and sample HTTP-to-gRPC endpoints.

### 6. Tests
From repository root, run:
```bash
dotnet test
```

---

If you want, I can:
- Expand test coverage (provide more unit/functional tests).
- Add CI pipeline (GitHub Actions) to build/test and publish images.
- Provide additional reporting endpoints (e.g., most-borrowed books, pages/day estimator).
- Walk you step-by-step through running and customizing this code locally.



## Added by request
- Database initializer and seed data for each service (auto-run on startup)
- Keycloak realm export (keycloak/realm-export.json) to import into Keycloak
- JWT authentication middleware placeholders for each service (configure Authority/Audience in appsettings or env)
- In-memory repository unit tests (xUnit)
- GitHub Actions CI workflow to build and run tests


## RabbitMQ / CQRS
- MassTransit configured to use RabbitMQ. Set `RABBITMQ_URL` env variable (e.g., CloudAMQP URL) in docker-compose or environment.
- LendingService exposes HTTP endpoints that act as write-side (Commands) using MediatR and publish `BookBorrowedEvent` to the bus.
