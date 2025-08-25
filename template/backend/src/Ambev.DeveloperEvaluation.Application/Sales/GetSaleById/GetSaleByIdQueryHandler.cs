using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSaleById
{
    public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleDto>
    {
        private readonly ISaleRepository _repository;

        public GetSaleByIdQueryHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<SaleDto> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var sale = await _repository.GetByIdAsync(request.SaleId, cancellationToken);
            if (sale == null)
                return null!;

            return new SaleDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                ClientName = sale.ClientName,
                Branch = sale.Branch,
                TotalAmount = sale.TotalAmount,
                Status = sale.Status.ToString(),
                Items = sale.Items.Select(i => new SaleDto.SaleItemDto
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    Total = i.Total
                }).ToList()
            };
        }
    }
}
