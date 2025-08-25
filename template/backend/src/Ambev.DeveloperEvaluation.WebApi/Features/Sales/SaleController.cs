using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.FinalizeSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSaleById;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/sales")]
    public class SaleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Finalize([FromBody] FinalizeSaleCommand command)
        {
            var saleId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = saleId }, null);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateSaleCommand command)
        {
            command.Id = id;

            var updatedId = await _mediator.Send(command);
            return Ok(new { id = updatedId });
        }

        [HttpGet]
        public async Task<IActionResult> Get(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int size = 10,
        [FromQuery(Name = "_order")] string? order = null,
        [FromQuery] string? saleNumber = null,
        [FromQuery] string? clientName = null,
        [FromQuery] string? branch = null,
        [FromQuery(Name = "_minDate")] DateTime? minDate = null,
        [FromQuery(Name = "_maxDate")] DateTime? maxDate = null)
        {
            var query = new GetSalesQuery
            {
                Page = page,
                Size = size,
                Order = order,
                SaleNumber = saleNumber,
                ClientName = clientName,
                Branch = branch,
                MinDate = minDate,
                MaxDate = maxDate
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _mediator.Send(new CancelSaleCommand(id));
            return result ? Ok() : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetSaleByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }
    }
}
