using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<(Sale[] Items, int Total)> SearchAsync(
            string? saleNumber,
            string? clientName,
            string? branch,
            DateTime? minDate,
            DateTime? maxDate,
            string? order,
            int page,
            int size,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Sales
                .AsNoTracking()
                .Include(s => s.Items)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(saleNumber))
                query = query.Where(s => EF.Functions.ILike(s.SaleNumber, $"%{saleNumber}%"));

            if (!string.IsNullOrWhiteSpace(clientName))
                query = query.Where(s => EF.Functions.ILike(s.ClientName, $"%{clientName}%"));

            if (!string.IsNullOrWhiteSpace(branch))
                query = query.Where(s => EF.Functions.ILike(s.Branch, $"%{branch}%"));

            if (minDate.HasValue)
                query = query.Where(s => s.SaleDate >= minDate.Value.Date);

            if (maxDate.HasValue)
                query = query.Where(s => s.SaleDate <= maxDate.Value.Date);

            query = ApplyOrdering(query, order);

            var total = await query.CountAsync(cancellationToken);

            var skip = (page <= 1 ? 0 : (page - 1) * size);
            var items = await query
                .Skip(skip)
                .Take(size)
                .ToArrayAsync(cancellationToken);

            return (items, total);
        }

        private static IQueryable<Sale> ApplyOrdering(IQueryable<Sale> query, string? order)
        {
            if (string.IsNullOrWhiteSpace(order))
                return query.OrderByDescending(s => s.SaleDate).ThenBy(s => s.SaleNumber);

            IOrderedQueryable<Sale>? ordered = null;

            foreach (var part in order.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var tokens = part.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var field = tokens[0].ToLowerInvariant();
                var desc = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

                if (ordered is null)
                {
                    ordered = field switch
                    {
                        "salenumber" => desc ? query.OrderByDescending(s => s.SaleNumber) : query.OrderBy(s => s.SaleNumber),
                        "saledate" => desc ? query.OrderByDescending(s => s.SaleDate) : query.OrderBy(s => s.SaleDate),
                        "clientname" => desc ? query.OrderByDescending(s => s.ClientName) : query.OrderBy(s => s.ClientName),
                        "branch" => desc ? query.OrderByDescending(s => s.Branch) : query.OrderBy(s => s.Branch),
                        "totalamount" => desc ? query.OrderByDescending(s => s.TotalAmount) : query.OrderBy(s => s.TotalAmount),
                        _ => desc ? query.OrderByDescending(s => s.SaleDate) : query.OrderBy(s => s.SaleDate)
                    };
                }
                else
                {
                    ordered = field switch
                    {
                        "salenumber" => desc ? ordered.ThenByDescending(s => s.SaleNumber) : ordered.ThenBy(s => s.SaleNumber),
                        "saledate" => desc ? ordered.ThenByDescending(s => s.SaleDate) : ordered.ThenBy(s => s.SaleDate),
                        "clientname" => desc ? ordered.ThenByDescending(s => s.ClientName) : ordered.ThenBy(s => s.ClientName),
                        "branch" => desc ? ordered.ThenByDescending(s => s.Branch) : ordered.ThenBy(s => s.Branch),
                        "totalamount" => desc ? ordered.ThenByDescending(s => s.TotalAmount) : ordered.ThenBy(s => s.TotalAmount),
                        _ => desc ? ordered.ThenByDescending(s => s.SaleDate) : ordered.ThenBy(s => s.SaleDate)
                    };
                }
            }

            return ordered ?? query.OrderByDescending(s => s.SaleDate).ThenBy(s => s.SaleNumber);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await _context.Sales.Include(s => s.Items)
                                           .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            if (sale is null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}