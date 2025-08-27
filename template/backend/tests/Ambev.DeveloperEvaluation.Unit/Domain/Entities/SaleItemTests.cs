using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTests
    {
        [Fact]
        public void ApplyDiscount_DeveAplicarZero_QuandoQuantidadeMenorQue4()
        {
            var item = new SaleItem
            {
                ProductName = "Produto A",
                Quantity = 3,
                UnitPrice = 100m
            };

            item.ApplyDiscount();

            item.Discount.Should().Be(0m);
            item.Total.Should().Be(300m);
            item.IsValid().Should().BeTrue();
        }

        [Fact]
        public void ApplyDiscount_DeveAplicar10PorCento_QuandoQuantidadeEntre4e9()
        {
            var item = new SaleItem
            {
                ProductName = "Produto B",
                Quantity = 4,
                UnitPrice = 50m
            };

            item.ApplyDiscount();

            var desconto = 4 * 50m * 0.10m;
            item.Discount.Should().Be(desconto);
            item.Total.Should().Be(200m - desconto);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(20)]
        public void ApplyDiscount_DeveAplicar20PorCento_QuandoQuantidadeEntre10e20(int qty)
        {
            var item = new SaleItem
            {
                ProductName = "Produto C",
                Quantity = qty,
                UnitPrice = 10m
            };

            item.ApplyDiscount();

            var desconto = qty * 10m * 0.20m;
            item.Discount.Should().Be(desconto);
            item.Total.Should().Be(qty * 10m - desconto);
        }

        [Fact]
        public void ApplyDiscount_DeveLancar_QuandoQuantidadeMaiorQue20()
        {
            var item = new SaleItem
            {
                ProductName = "Produto D",
                Quantity = 21,
                UnitPrice = 10m
            };

            Action act = () => item.ApplyDiscount();

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("*acima de 20*");

            item.IsValid().Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void IsValid_DeveSerFalso_QuandoQuantidadeZeroOuNegativa(int qty)
        {
            var item = new SaleItem
            {
                ProductName = "Produto E",
                Quantity = qty,
                UnitPrice = 10m
            };

            item.IsValid().Should().BeFalse();
        }
    }
}
