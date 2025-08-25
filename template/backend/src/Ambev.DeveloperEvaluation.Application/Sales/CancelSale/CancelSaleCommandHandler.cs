using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, bool>
    {
        private readonly ISaleRepository _repository;

        public CancelSaleCommandHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _repository.GetByIdAsync(request.SaleId, cancellationToken);
            if (sale == null)
                return false;

            if (sale.Status == Domain.Enums.SaleStatus.Cancelled)
                return false;

            sale.Cancel();
            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
