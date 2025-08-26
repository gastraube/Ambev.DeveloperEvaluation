using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleDeletedEventHandler : INotificationHandler<SaleDeletedEvent>
    {
        private readonly ILogger<SaleDeletedEventHandler> _logger;
        public SaleDeletedEventHandler(ILogger<SaleDeletedEventHandler> logger) => _logger = logger;

        public Task Handle(SaleDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Event SaleDeleted | SaleId: {SaleId} | Number: {Number}",
                notification.SaleId, notification.SaleNumber);
            return Task.CompletedTask;
        }
    }
}
