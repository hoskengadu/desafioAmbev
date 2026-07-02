using DeveloperStore.Sales.Application.Abstractions;
using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Domain.Common;
using DeveloperStore.Sales.Domain.Sales;
using DeveloperStore.Sales.Domain.ValueObjects;

namespace DeveloperStore.Sales.Application.Sales;

public sealed class SaleService
{
    private readonly ISaleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    public SaleService(ISaleRepository repository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<Guid>> CreateAsync(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = new Sale(
            new SaleNumber(command.SaleNumber),
            command.SaleDate,
            new Customer(command.CustomerId, command.CustomerName),
            new Branch(command.BranchId, command.BranchName),
            command.Items.Select(x => new SaleItem(x.ProductId, x.ProductName, x.Quantity, new Money(x.UnitPrice))));

        await _repository.AddAsync(sale, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _eventPublisher.PublishAsync(sale.DomainEvents, cancellationToken);
        sale.ClearDomainEvents();
        return Result<Guid>.Success(sale.Id);
    }

    public async Task<Result> UpdateAsync(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (sale is null) return Result.Failure("Sale not found.");

        sale.Update(
            new SaleNumber(command.SaleNumber),
            command.SaleDate,
            new Customer(command.CustomerId, command.CustomerName),
            new Branch(command.BranchId, command.BranchName),
            command.Items.Select(x => new SaleItem(x.ProductId, x.ProductName, x.Quantity, new Money(x.UnitPrice))));

        _repository.Update(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _eventPublisher.PublishAsync(sale.DomainEvents, cancellationToken);
        sale.ClearDomainEvents();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(id, cancellationToken);
        if (sale is null) return Result.Failure("Sale not found.");
        _repository.Remove(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> CancelAsync(Guid id, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(id, cancellationToken);
        if (sale is null) return Result.Failure("Sale not found.");
        sale.Cancel();
        _repository.Update(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _eventPublisher.PublishAsync(sale.DomainEvents, cancellationToken);
        sale.ClearDomainEvents();
        return Result.Success();
    }

    public async Task<Result> CancelItemAsync(Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(id, cancellationToken);
        if (sale is null) return Result.Failure("Sale not found.");
        sale.CancelItem(itemId);
        _repository.Update(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _eventPublisher.PublishAsync(sale.DomainEvents, cancellationToken);
        sale.ClearDomainEvents();
        return Result.Success();
    }

    public async Task<Result<SaleResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await GetAsync(await _repository.GetByIdAsync(id, cancellationToken));

    public async Task<Result<SaleResponse>> GetByNumberAsync(string saleNumber, CancellationToken cancellationToken)
        => await GetAsync(await _repository.GetByNumberAsync(saleNumber, cancellationToken));

    public async Task<Result<IReadOnlyCollection<SaleResponse>>> GetAllAsync(CancellationToken cancellationToken)
        => Result<IReadOnlyCollection<SaleResponse>>.Success((await _repository.GetAllAsync(cancellationToken)).Select(SaleMapper.ToResponse).ToArray());

    private static Task<Result<SaleResponse>> GetAsync(Sale? sale)
        => Task.FromResult(sale is null ? Result<SaleResponse>.Failure("Sale not found.") : Result<SaleResponse>.Success(SaleMapper.ToResponse(sale)));
}
