using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, PagedResult<SaleListItemDto>>
    {
        private readonly ISaleRepository _repository;

        public GetSalesQueryHandler(ISaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<SaleListItemDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var size = request.Size <= 0 ? 10 : request.Size;

            var (items, total) = await _repository.SearchAsync(
                request.SaleNumber,
                request.ClientName,
                request.Branch,
                request.MinDate,
                request.MaxDate,
                request.Order,
                page,
                size,
                cancellationToken);

            var data = items.Select(s => new SaleListItemDto
            {
                Id = s.Id,
                SaleNumber = s.SaleNumber,
                SaleDate = s.SaleDate,
                ClientName = s.ClientName,
                Branch = s.Branch,
                TotalAmount = s.TotalAmount,
                Status = s.Status.ToString(),
                ItemsCount = s.Items?.Count ?? 0
            }).ToList();

            var totalPages = (int)Math.Ceiling(total / (double)size);

            return new PagedResult<SaleListItemDto>
            {
                Data = data,
                TotalItems = total,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }
    }
}
