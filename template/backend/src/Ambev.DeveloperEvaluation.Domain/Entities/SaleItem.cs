using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; private set; }

        public decimal Total => Math.Round((UnitPrice * Quantity) - Discount, 2);

        public Guid SaleId { get; set; }

        public Sale? Sale { get; set; }

        public void ApplyDiscount()
        {
            if (Quantity >= 10 && Quantity <= 20)
            {
                Discount = UnitPrice * Quantity * 0.2m;
            }
            else if (Quantity >= 4 && Quantity < 10)
            {
                Discount = UnitPrice * Quantity * 0.1m;
            }
            else if (Quantity > 20)
            {
                throw new InvalidOperationException("Quantidade acima de 20 não é permitida.");
            }
            else
            {
                Discount = 0;
            }
        }

        public bool IsValid()
        {
            return Quantity > 0 && Quantity <= 20;
        }
    }
}
