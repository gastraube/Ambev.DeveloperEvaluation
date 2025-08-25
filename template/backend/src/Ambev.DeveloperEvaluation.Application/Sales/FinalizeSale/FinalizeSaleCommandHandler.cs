using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.FinalizeSale
{
    public class FinalizeSaleCommandHandler : IRequestHandler<FinalizeSaleCommand, Guid>
    {
        private readonly ISaleRepository _repository;

        public FinalizeSaleCommandHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(FinalizeSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = new Sale
            {
                SaleNumber = request.SaleNumber,
                SaleDate = request.Date,
                ClientName = request.Customer,
                Branch = request.Branch,
                Items = request.Items.Select(item => new SaleItem
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            sale.FinalizeSale();

            await _repository.AddAsync(sale);
            return sale.Id;
        }
    }
}
