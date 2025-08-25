using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(x => x.SaleNumber).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ClientName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Branch).NotEmpty().MaximumLength(100);
            RuleFor(x => x.SaleDate).NotEmpty();

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("A venda deve possuir pelo menos um item.");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductName).NotEmpty().MaximumLength(200);
                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantidade deve ser pelo menos 1.")
                    .LessThanOrEqualTo(20).WithMessage("Quantidade máxima por item é 20.");
                items.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0).WithMessage("Preço unitário deve ser maior que 0.");
            });
        }
    }
}
