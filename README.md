# DeveloperStore Sales API

API REST de vendas desenvolvida com .NET 9, DDD, Clean Architecture e CQRS.

## Arquitetura

- `DeveloperStore.Sales.Domain`: agregado `Sale`, entidades, value objects e regras de negócio
- `DeveloperStore.Sales.Application`: comandos, queries, validações e orquestração de casos de uso
- `DeveloperStore.Sales.Infrastructure`: EF Core, SQL Server, repositórios, UoW e publisher de eventos por log
- `DeveloperStore.Sales.Api`: controllers, middleware global de exceções, Swagger e Serilog
- `tests/*`: testes unitários, de integração e de arquitetura

## Regras de negócio

- Menos de 4 itens idênticos: sem desconto
- De 4 a 9 itens: 10% de desconto
- De 10 a 20 itens: 20% de desconto
- Acima de 20 itens: exceção de domínio

## Endpoints

- `POST /sales`
- `GET /sales`
- `GET /sales/{id}`
- `PUT /sales/{id}`
- `DELETE /sales/{id}`
- `PATCH /sales/{id}/cancel`
- `PATCH /sales/{id}/items/{itemId}/cancel`

## Como executar

### Local

```powershell
dotnet run --project src/DeveloperStore.Sales.Api
```

### Docker

```powershell
docker compose up --build
```

## Testes

```powershell
dotnet test DeveloperStore.Sales.slnx
```

## Banco de dados

Use SQL Server. A connection string padrão está em `appsettings.json` e no `docker-compose.yml`.

## Pontos futuros

- Finalizar integração com `MediatR` no pipeline de comandos e queries
- Completar testes de integração com `Testcontainers`
- Gerar migrations e aplicar seed de desenvolvimento
- Publicar a solução em repositório GitHub público para avaliação
