using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleItemCancelledEventHandler : INotificationHandler<SaleItemCancelledEvent>
    {
        private readonly ILogger<SaleItemCancelledEventHandler> _logger;

        public SaleItemCancelledEventHandler(ILogger<SaleItemCancelledEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SaleItemCancelledEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event SaleItemCancelled | SaleId: {SaleId} | Product: {Product} | Qty: {Qty}",
                notification.SaleId, notification.ProductName, notification.Quantity);
            return Task.CompletedTask;
        }
    }
}
