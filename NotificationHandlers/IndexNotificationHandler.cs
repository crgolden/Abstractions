namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class IndexNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : IndexNotification
    {
        private readonly ILogger<IndexNotificationHandler<TNotification>> _logger;

        protected IndexNotificationHandler(ILogger<IndexNotificationHandler<TNotification>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.IndexStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Searching request {Request} at {Time}",
                        args: new object[] { notification.Request, DateTime.UtcNow });
                    break;
                case EventIds.IndexEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Found result {Result} at {Time}",
                        args: new object[] { notification.Result, DateTime.UtcNow });
                    break;
                case EventIds.IndexError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error searching request {Request} at {Time}",
                        args: new object[] { notification.Request, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
