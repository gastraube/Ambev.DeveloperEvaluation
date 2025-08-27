using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Shared
{
    public static class SaleFaker
    {
        public static Faker<Sale> Create(int items = 1)
            => new Faker<Sale>()
                .CustomInstantiator(f =>
                {
                    var sale = new Sale
                    {
                        SaleNumber = f.Commerce.Ean13(),
                        SaleDate = f.Date.RecentOffset(10).UtcDateTime,
                        ClientName = f.Person.FullName,
                        Branch = f.Address.City()
                    };

                    var sid = sale.Id; // se BaseEntity setar Guid no ctor

                    sale.Items = Enumerable.Range(0, items)
                        .Select(_ => SaleItemFaker.Create(sid).Generate())
                        .ToList();

                    return sale;
                });
    }
}
