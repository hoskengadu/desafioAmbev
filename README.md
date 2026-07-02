# DeveloperStore Sales API

Base inicial de uma API de vendas em .NET 9 com Clean Architecture e DDD.

## Estrutura
- `src/DeveloperStore.Sales.Domain`
- `src/DeveloperStore.Sales.Application`
- `src/DeveloperStore.Sales.Infrastructure`
- `src/DeveloperStore.Sales.Api`

## Execução
```powershell
dotnet run --project src/DeveloperStore.Sales.Api
```

## Docker
```powershell
docker compose up --build
```

## Observação
Esta base foi montada no workspace, mas a restauração completa de pacotes externos está bloqueada no ambiente atual.
