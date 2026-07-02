using DeveloperStore.Sales.Application.Sales;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Sales.Api.Controllers;

[ApiController]
[Route("sales")]
public sealed class SalesController : ControllerBase
{
    private readonly SaleService _service;

    public SalesController(SaleService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : BadRequest(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok((await _service.GetAllAsync(cancellationToken)).Value);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        => Ok((await _service.GetByIdAsync(id, cancellationToken)).Value);

    [HttpGet("number/{saleNumber}")]
    public async Task<IActionResult> GetByNumber(string saleNumber, CancellationToken cancellationToken)
        => Ok((await _service.GetByNumberAsync(saleNumber, cancellationToken)).Value);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleCommand command, CancellationToken cancellationToken)
        => Ok(await _service.UpdateAsync(command with { Id = id }, cancellationToken));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => Ok(await _service.DeleteAsync(id, cancellationToken));

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        => Ok(await _service.CancelAsync(id, cancellationToken));

    [HttpPatch("{id:guid}/items/{itemId:guid}/cancel")]
    public async Task<IActionResult> CancelItem(Guid id, Guid itemId, CancellationToken cancellationToken)
        => Ok(await _service.CancelItemAsync(id, itemId, cancellationToken));
}
