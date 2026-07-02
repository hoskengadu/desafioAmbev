using DeveloperStore.Sales.Application.Sales;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.Api.Controllers;

[ApiController]
[Route("sales")]
public sealed class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : result.Error?.Contains("already exists", StringComparison.OrdinalIgnoreCase) == true
                ? Conflict(result.Error)
                : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSalesQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSaleByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("number/{saleNumber}")]
    public async Task<IActionResult> GetByNumber(string saleNumber, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSaleByNumberQuery(saleNumber), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return result.IsSuccess
            ? NoContent()
            : result.Error?.Contains("already exists", StringComparison.OrdinalIgnoreCase) == true
                ? Conflict(result.Error)
                : NotFound(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteSaleCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelSaleCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [HttpPatch("{id:guid}/items/{itemId:guid}/cancel")]
    public async Task<IActionResult> CancelItem(Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelSaleItemCommand(id, itemId), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }
}
