using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact]
        public void FinalizeSale_DeveAplicarDescontosECalcularTotal()
        {
            var sale = new Sale
            {
                SaleNumber = "S-001",
                SaleDate = DateTime.UtcNow,
                ClientName = "Cliente A",
                Branch = "Filial 01"
            };

            sale.Items.Add(new SaleItem { ProductName = "A", Quantity = 3, UnitPrice = 100m }); // 0% -> 300
            sale.Items.Add(new SaleItem { ProductName = "B", Quantity = 4, UnitPrice = 50m }); // 10% -> 180
            sale.Items.Add(new SaleItem { ProductName = "C", Quantity = 10, UnitPrice = 10m }); // 20% ->  80

            sale.FinalizeSale();

            sale.Items[0].Discount.Should().Be(0m);
            sale.Items[0].Total.Should().Be(300m);

            sale.Items[1].Discount.Should().Be(20m);
            sale.Items[1].Total.Should().Be(180m);

            sale.Items[2].Discount.Should().Be(20m);
            sale.Items[2].Total.Should().Be(80m);

            sale.TotalAmount.Should().Be(300m + 180m + 80m);
            sale.Status.Should().Be(SaleStatus.Active);
            sale.IsValid().Should().BeTrue();
        }

        [Fact]
        public void FinalizeSale_DeveLancar_QuandoTemItemInvalido()
        {
            var sale = new Sale
            {
                SaleNumber = "S-002",
                SaleDate = DateTime.UtcNow,
                ClientName = "Cliente B",
                Branch = "Filial 01"
            };

            sale.Items.Add(new SaleItem { ProductName = "A", Quantity = 1, UnitPrice = 100m });
            sale.Items.Add(new SaleItem { ProductName = "B", Quantity = 21, UnitPrice = 10m }); // inválido

            Action act = () => sale.FinalizeSale();

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("*inválidos*");
        }

        [Fact]
        public void Cancel_DeveAlterarStatusEAtualizarUpdatedAt()
        {
            var sale = new Sale
            {
                SaleNumber = "S-003",
                SaleDate = DateTime.UtcNow,
                ClientName = "Cliente C",
                Branch = "Filial 01"
            };

            sale.Items.Add(new SaleItem { ProductName = "A", Quantity = 4, UnitPrice = 50m });
            sale.FinalizeSale();

            sale.Cancel();

            sale.Status.Should().Be(SaleStatus.Cancelled);
            sale.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void UpdateItems_DeveReaplicarDescontosERecalcularTotal()
        {
            var sale = new Sale
            {
                SaleNumber = "S-004",
                SaleDate = DateTime.UtcNow,
                ClientName = "Cliente D",
                Branch = "Filial 01"
            };

            sale.Items.Add(new SaleItem { ProductName = "A", Quantity = 3, UnitPrice = 100m });
            sale.FinalizeSale();
            var totalAntes = sale.TotalAmount;

            var novos = new[]
            {
            new SaleItem { ProductName = "B", Quantity = 4, UnitPrice = 50m },  // 10% -> 180
            new SaleItem { ProductName = "C", Quantity = 10, UnitPrice = 10m }  // 20% ->  80
        };

            sale.UpdateItems(novos);

            sale.TotalAmount.Should().Be(180m + 80m);
            sale.TotalAmount.Should().NotBe(totalAntes);
            sale.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public void CalculateTotal_DeveSomarTotaisDosItens()
        {
            var sale = new Sale
            {
                SaleNumber = "S-005",
                SaleDate = DateTime.UtcNow,
                ClientName = "Cliente E",
                Branch = "Filial 01"
            };

            var i1 = new SaleItem { ProductName = "B", Quantity = 4, UnitPrice = 50m };  // 10% -> 180
            var i2 = new SaleItem { ProductName = "C", Quantity = 10, UnitPrice = 10m }; // 20% ->  80
            i1.ApplyDiscount();
            i2.ApplyDiscount();
            sale.Items.Add(i1);
            sale.Items.Add(i2);

            sale.CalculateTotal();

            sale.TotalAmount.Should().Be(180m + 80m);
        }
    }
}
