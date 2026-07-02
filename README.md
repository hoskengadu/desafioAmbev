# DeveloperStore Sales API

Sales API built with .NET 9, DDD, Clean Architecture, CQRS, EF Core and SQL Server.

## Overview

This solution manages sales records using the External Identity Pattern for Customer, Branch and Product data.

The main goal is to keep the domain independent, expressive and ready for production-style maintenance.

## Architecture

- `DeveloperStore.Sales.Domain`
  - Aggregate Root: `Sale`
  - Entity: `SaleItem`
  - Value Objects: `Money`, `Percentage`, `SaleNumber`, `Customer`, `Branch`
  - Domain rules and domain events
- `DeveloperStore.Sales.Application`
  - CQRS commands and queries
  - FluentValidation validators
  - MediatR handlers
  - Response models and result pattern
- `DeveloperStore.Sales.Infrastructure`
  - EF Core mapping
  - SQL Server persistence
  - Repository pattern
  - Unit of Work
  - Structured domain event logging
- `DeveloperStore.Sales.CrossCutting`
  - Composition root
  - Service registration
- `DeveloperStore.Sales.Api`
  - REST controllers
  - Global exception middleware
  - Swagger/OpenAPI
  - Serilog
- `tests`
  - Unit tests
  - Integration tests
  - Architecture tests

## Business Rules

- Quantities below 4 have no discount
- Quantities from 4 to 9 have 10% discount
- Quantities from 10 to 20 have 20% discount
- Quantities above 20 are not allowed
- Sale cancelation is supported
- Sale item cancelation is supported

## Endpoints

- `POST /sales`
- `GET /sales`
- `GET /sales/{id}`
- `GET /sales/number/{saleNumber}`
- `PUT /sales/{id}`
- `DELETE /sales/{id}`
- `PATCH /sales/{id}/cancel`
- `PATCH /sales/{id}/items/{itemId}/cancel`

Examples are available in [`DeveloperStore.Sales.Api.http`](src/DeveloperStore.Sales.Api/DeveloperStore.Sales.Api.http).

## How to Run

### Local

```powershell
dotnet run --project src/DeveloperStore.Sales.Api
```

### Docker

```powershell
docker compose up --build
```

The compose file starts:
- API
- SQL Server

## Database

The solution is configured for SQL Server.

Connection string used by default:

```text
Server=localhost,1433;Database=DeveloperStoreSales;User Id=sa;Password=Your_password123;TrustServerCertificate=True
```

When running with Docker Compose, the API container overrides this value to use the `sqlserver` host on the compose network.

## Migrations

The application applies migrations on startup through the `CrossCutting` composition layer.

Recommended flow:

```powershell
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/DeveloperStore.Sales.Infrastructure --startup-project src/DeveloperStore.Sales.Api
dotnet ef database update --project src/DeveloperStore.Sales.Infrastructure --startup-project src/DeveloperStore.Sales.Api
```

The migration is also applied automatically when the API starts.

## Tests

### Unit tests

```powershell
dotnet test tests/DeveloperStore.Sales.UnitTests/DeveloperStore.Sales.UnitTests.csproj
```

### Integration tests

```powershell
dotnet test tests/DeveloperStore.Sales.IntegrationTests/DeveloperStore.Sales.IntegrationTests.csproj
```

### Architecture tests

```powershell
dotnet test tests/DeveloperStore.Sales.ArchitectureTests/DeveloperStore.Sales.ArchitectureTests.csproj
```

### Full suite

```powershell
dotnet test DeveloperStore.Sales.sln
```

## Quality Notes

- Nullable reference types enabled
- Implicit usings enabled
- Serilog for structured logging
- Global exception handling returning `ProblemDetails`
- CQRS through MediatR
- Domain rules remain inside the domain model
- API does not reference Infrastructure directly; the composition root handles it
- Test suite split into unit, integration and architecture layers
- Target coverage is 80% or higher

## Suggested Flow

1. Client calls API
2. Controller sends a command or query through MediatR
3. Validators run in the pipeline
4. Handler invokes domain behavior
5. Repository and Unit of Work persist data
6. Domain events are logged
7. API returns a REST response

## Future Improvements

- Add authentication and authorization
- Add paging and filtering for list endpoints
- Add optimistic concurrency
- Expand integration coverage with more negative cases
- Add OpenAPI examples and response schemas

## Repository Requirement

The challenge requires a public GitHub repository link for evaluation.
After publishing the code, send the repository URL.
