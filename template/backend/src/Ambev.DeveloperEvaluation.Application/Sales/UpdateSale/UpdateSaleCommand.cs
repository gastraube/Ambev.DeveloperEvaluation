using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;

        public List<UpdateSaleItemDto> Items { get; set; } = new();
    }
}
