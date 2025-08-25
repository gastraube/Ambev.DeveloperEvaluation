using Ambev.DeveloperEvaluation.Application.Sales.FinalizeSale;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok();
        }
    }
}
