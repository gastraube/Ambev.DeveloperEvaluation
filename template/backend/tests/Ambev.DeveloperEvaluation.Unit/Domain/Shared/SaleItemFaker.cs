using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Shared
{
    public static class SaleItemFaker
    {
        public static Faker<SaleItem> Create(Guid? saleId = null)
            => new Faker<SaleItem>()
                .CustomInstantiator(f => new SaleItem
                {
                    ProductName = f.Commerce.ProductName(),
                    Quantity = f.Random.Int(1, 20),
                    UnitPrice = Math.Round(f.Random.Decimal(10, 1000), 2),
                    SaleId = saleId ?? Guid.NewGuid()
                });

        public static SaleItem Build(int quantity, decimal unitPrice, string? productName = null)
            => new SaleItem
            {
                ProductName = productName ?? "Produto X",
                Quantity = quantity,
                UnitPrice = unitPrice
            };
    }
}
