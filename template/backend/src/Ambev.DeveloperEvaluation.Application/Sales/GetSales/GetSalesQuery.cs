using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesQuery : IRequest<PagedResult<SaleListItemDto>>
    {
        public int Page { get; set; } = 1;   
        public int Size { get; set; } = 10;

        public string? Order { get; set; }

        public string? SaleNumber { get; set; }
        public string? ClientName { get; set; }
        public string? Branch { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
