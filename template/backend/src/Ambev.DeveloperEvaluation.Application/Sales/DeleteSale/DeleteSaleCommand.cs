using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public sealed class DeleteSaleCommand : IRequest<bool>
    {
        public Guid Id { get; }
        public DeleteSaleCommand(Guid id) => Id = id;
    }
}
