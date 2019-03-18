namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Shared;

    public abstract class ListNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : ListNotification
    {
        private readonly ILogger<ListNotificationHandler<TNotification>> _logger;

        protected ListNotificationHandler(ILogger<ListNotificationHandler<TNotification>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.ListStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Searching request {@Request} at {@Time}",
                        args: new object[] { notification.Request, DateTime.UtcNow });
                    break;
                case EventIds.ListEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Found result {@Result} at {@Time}",
                        args: new object[] { notification.Result, DateTime.UtcNow });
                    break;
                case EventIds.ListError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error searching request {@Request} at {@Time}",
                        args: new object[] { notification.Request, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
