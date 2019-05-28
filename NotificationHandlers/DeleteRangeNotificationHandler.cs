namespace Clarity.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Shared;

    [ExcludeFromCodeCoverage]
    public abstract class DeleteRangeNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : DeleteRangeNotification
    {
        private readonly ILogger<DeleteRangeNotificationHandler<TNotification>> _logger;

        protected DeleteRangeNotificationHandler(ILogger<DeleteRangeNotificationHandler<TNotification>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.DeleteRangeStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Deleting model(s) with key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DeleteRangeEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Deleted model(s) with key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DeleteRangeError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error deleting model(s) with key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
