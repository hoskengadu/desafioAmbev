using Ambev.DeveloperEvaluation.Application.Abstractions;
using Ambev.DeveloperEvaluation.Common;
using Ambev.DeveloperEvaluation.Domain.Sales;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, Result<Guid>>
{
    private readonly ISaleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    public CreateSaleHandler(ISaleRepository repository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<Guid>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.GetByNumberAsync(request.SaleNumber, cancellationToken) is not null)
        {
            return Result<Guid>.Failure("Sale number already exists.");
        }

        var sale = BuildSale(request);
        await _repository.AddAsync(sale, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _eventPublisher.PublishAsync(sale.DomainEvents, cancellationToken);
        sale.ClearDomainEvents();
        return Result<Guid>.Success(sale.Id);
    }

    private static Sale BuildSale(CreateSaleCommand command)
        => new(
            new SaleNumber(command.SaleNumber),
            command.SaleDate,
            new Customer(command.CustomerId, command.CustomerName),
            new Branch(command.BranchId, command.BranchName),
            command.Items.Select(x => new SaleItem(x.ProductId, x.ProductName, x.Quantity, new Money(x.UnitPrice))));
}

public sealed class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, Result>
{
    private readonly ISaleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    public UpdateSaleHandler(ISaleRepository repository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (sale is null) return Result.Failure("Sale not found.");

        var existingSale = await _repository.GetByNumberAsync(request.SaleNumber, cancellationToken);
        if (existingSale is not null && existingSale.Id != request.Id)
        {
            return Result.Failure("Sale number already exists.");
        }

        sale.Update(
            new SaleNumber(request.SaleNumber),
            request.SaleDate,
            new Customer(request.CustomerId, request.CustomerName),
            new Branch(request.BranchId, request.BranchName),
            request.Items.Select(x => new SaleItem(x.ProductId, x.ProductName, x.Quantity, new Money(x.UnitPrice))));
        _repository.Update(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _eventPublisher.PublishAsync(sale.DomainEvents, cancellationToken);
        sale.ClearDomainEvents();
        return Result.Success();
    }
}

public sealed class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, Result>
{
    private readonly ISaleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteSaleHandler(ISaleRepository repository, IUnitOfWork unitOfWork) { _repository = repository; _unitOfWork = unitOfWork; }
    public async Task<Result> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (sale is null) return Result.Failure("Sale not found.");
        _repository.Remove(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

public sealed class CancelSaleHandler : IRequestHandler<CancelSaleCommand, Result>
{
    private readonly ISaleRepository _repository; private readonly IUnitOfWork _unitOfWork; private readonly IEventPublisher _eventPublisher;
    public CancelSaleHandler(ISaleRepository repository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher) { _repository = repository; _unitOfWork = unitOfWork; _eventPublisher = eventPublisher; }
    public async Task<Result> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (sale is null) return Result.Failure("Sale not found.");
        sale.Cancel(); _repository.Update(sale); await _unitOfWork.SaveChangesAsync(cancellationToken); await _eventPublisher.PublishAsync(sale.DomainEvents, cancellationToken); sale.ClearDomainEvents(); return Result.Success();
    }
}

public sealed class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, Result>
{
    private readonly ISaleRepository _repository; private readonly IUnitOfWork _unitOfWork; private readonly IEventPublisher _eventPublisher;
    public CancelSaleItemHandler(ISaleRepository repository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher) { _repository = repository; _unitOfWork = unitOfWork; _eventPublisher = eventPublisher; }
    public async Task<Result> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (sale is null) return Result.Failure("Sale not found.");
        sale.CancelItem(request.ItemId); _repository.Update(sale); await _unitOfWork.SaveChangesAsync(cancellationToken); await _eventPublisher.PublishAsync(sale.DomainEvents, cancellationToken); sale.ClearDomainEvents(); return Result.Success();
    }
}

public sealed class GetSaleByIdHandler : IRequestHandler<GetSaleByIdQuery, Result<SaleResponse>>
{
    private readonly ISaleRepository _repository;
    public GetSaleByIdHandler(ISaleRepository repository) => _repository = repository;
    public async Task<Result<SaleResponse>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        => await HandlerMapping.Map(await _repository.GetByIdAsync(request.Id, cancellationToken));
}

public sealed class GetSaleByNumberHandler : IRequestHandler<GetSaleByNumberQuery, Result<SaleResponse>>
{
    private readonly ISaleRepository _repository;
    public GetSaleByNumberHandler(ISaleRepository repository) => _repository = repository;
    public async Task<Result<SaleResponse>> Handle(GetSaleByNumberQuery request, CancellationToken cancellationToken)
        => await HandlerMapping.Map(await _repository.GetByNumberAsync(request.SaleNumber, cancellationToken));
}

public sealed class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, Result<PagedResult<SaleResponse>>>
{
    private readonly ISaleRepository _repository;
    public GetAllSalesHandler(ISaleRepository repository) => _repository = repository;
    public async Task<Result<PagedResult<SaleResponse>>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(request.PageNumber, 1);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var searchRequest = new SaleSearchRequest(
            pageNumber,
            pageSize,
            request.SaleNumber,
            request.CustomerName,
            request.BranchName,
            request.Cancelled,
            request.SaleDateFrom,
            request.SaleDateTo,
            request.SortBy,
            request.SortDirection);

        var result = await _repository.SearchAsync(searchRequest, cancellationToken);
        var items = result.Items.Select(SaleMapper.ToResponse).ToArray();
        return Result<PagedResult<SaleResponse>>.Success(new PagedResult<SaleResponse>(items, pageNumber, pageSize, result.TotalCount));
    }
}

static class HandlerMapping
{
    public static Task<Result<SaleResponse>> Map(Sale? sale)
        => Task.FromResult(sale is null ? Result<SaleResponse>.Failure("Sale not found.") : Result<SaleResponse>.Success(SaleMapper.ToResponse(sale)));
}

