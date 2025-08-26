using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, Guid>
    {
        private readonly ISaleRepository _repository;

        public UpdateSaleCommandHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (sale is null)
                throw new InvalidOperationException("Venda não encontrada.");

            sale.SaleNumber = request.SaleNumber;
            sale.SaleDate = request.SaleDate;
            sale.ClientName = request.ClientName;
            sale.Branch = request.Branch;

            sale.Items = request.Items.Select(i => new SaleItem
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            sale.FinalizeSale();

            sale.AddDomainEvent(new SaleModifiedEvent(sale.Id, sale.SaleNumber, sale.TotalAmount));
            await _repository.SaveChangesAsync(cancellationToken);
            return sale.Id;
        }
    }
}
