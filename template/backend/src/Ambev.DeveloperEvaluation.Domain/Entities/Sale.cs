using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public decimal TotalAmount { get; private set; }
        public SaleStatus Status { get; private set; } = SaleStatus.Active;
        public List<SaleItem> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Sale()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void CalculateTotal()
        {
            TotalAmount = Items.Sum(i => i.Total);
        }

        public void Cancel()
        {
            Status = SaleStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new SaleCancelledEvent(Id, SaleNumber));
        }

        public bool IsValid()
        {
            return Items.All(i => i.IsValid());
        }

        public void FinalizeSale()
        {
            if (!IsValid())
                throw new InvalidOperationException("Itens inválidos na venda.");

            Items.ForEach(i => i.ApplyDiscount());
            CalculateTotal();
        }

        public void UpdateItems(IEnumerable<SaleItem> newItems)
        {
            Items = newItems.ToList();
            Items.ForEach(i => i.ApplyDiscount());
            CalculateTotal();
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new SaleModifiedEvent(Id, SaleNumber, TotalAmount));
        }
    }

}
