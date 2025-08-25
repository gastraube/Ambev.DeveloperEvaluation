using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.FinalizeSale
{
    public class FinalizeSaleCommand : IRequest<Guid>
    {
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public string Branch { get; set; }
        public List<SaleItemDto> Items { get; set; }

        public class SaleItemDto
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
