# DeveloperStore Sales API

Sales API built with .NET 9, DDD, Clean Architecture, CQRS, EF Core and SQL Server.

## Overview

This solution manages sales records using the External Identity Pattern for Customer, Branch and Product data.

The main goal is to keep the domain independent, expressive and ready for production-style maintenance.

## Architecture

- `Ambev.DeveloperEvaluation.Domain`
  - Aggregate Root: `Sale`
  - Entity: `SaleItem`
  - Value Objects: `Money`, `Percentage`, `SaleNumber`, `Customer`, `Branch`
  - Domain rules and domain events
- `Ambev.DeveloperEvaluation.Application`
  - CQRS commands and queries
  - FluentValidation validators
  - MediatR handlers
  - Response models and result pattern
- `Ambev.DeveloperEvaluation.Common`
  - Cross-cutting result types
  - MediatR validation behavior
- `Ambev.DeveloperEvaluation.ORM`
  - EF Core mapping
  - SQL Server persistence
  - Repository pattern
  - Unit of Work
  - Structured domain event logging
- `Ambev.DeveloperEvaluation.IoC`
  - Composition root
  - Service registration
- `Ambev.DeveloperEvaluation.WebApi`
  - REST controllers
  - Global exception middleware
  - Swagger/OpenAPI
  - Serilog
- `tests`
  - Functional tests
  - Integration tests
  - Unit tests
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
  - Supports pagination, filtering and sorting through query string parameters:
    - `PageNumber`
    - `PageSize`
    - `SaleNumber`
    - `CustomerName`
    - `BranchName`
    - `Cancelled`
    - `SaleDateFrom`
    - `SaleDateTo`
    - `SortBy`
    - `SortDirection`
  - Returns a paged payload with `Items`, `PageNumber`, `PageSize`, `TotalCount` and `TotalPages`.
- `GET /sales/{id}`
- `GET /sales/number/{saleNumber}`
- `PUT /sales/{id}`
- `DELETE /sales/{id}`
- `PATCH /sales/{id}/cancel`
- `PATCH /sales/{id}/items/{itemId}/cancel`

Examples are available in [`Ambev.DeveloperEvaluation.WebApi.http`](src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.http).

## How to Run

### Local

```powershell
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

### Docker

```powershell
docker compose up --build
```

The compose file starts:
- Web API
- SQL Server

## Database

The solution is configured for SQL Server.

Connection string used by default:

```text
Server=localhost,1433;Database=DeveloperStoreSales;User Id=sa;Password=Your_password123;TrustServerCertificate=True
```

When running with Docker Compose, the API container overrides this value to use the `sqlserver` host on the compose network.

## Migrations

The application applies migrations on startup through the `IoC` composition layer.

Recommended flow:

```powershell
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

The migration is also applied automatically when the API starts.

## Tests

### Unit tests

```powershell
dotnet test tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj
```

### Integration tests

```powershell
dotnet test tests/Ambev.DeveloperEvaluation.Integration/Ambev.DeveloperEvaluation.Integration.csproj
```

### Functional tests

```powershell
dotnet test tests/Ambev.DeveloperEvaluation.Functional/Ambev.DeveloperEvaluation.Functional.csproj
```

### Architecture tests

```powershell
dotnet test tests/Ambev.DeveloperEvaluation.ArchitectureTests/Ambev.DeveloperEvaluation.ArchitectureTests.csproj
```

### Full suite

```powershell
dotnet test Ambev.DeveloperEvaluation.sln
```

## Quality Notes

- Nullable reference types enabled
- Implicit usings enabled
- Serilog for structured logging
- Global exception handling returning `ProblemDetails`
- CQRS through MediatR
- Domain rules remain inside the domain model
- API does not reference ORM directly; the composition root handles it
- Test suite split into functional, integration, unit and architecture layers
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
- Add optimistic concurrency
- Expand integration coverage with more negative cases
- Add OpenAPI examples and response schemas

## Repository Requirement

The challenge requires a public GitHub repository link for evaluation.
After publishing the code, send the repository URL.
