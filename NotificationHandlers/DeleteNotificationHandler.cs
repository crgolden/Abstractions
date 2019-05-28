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
    public abstract class DeleteNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : DeleteNotification
    {
        private readonly ILogger<DeleteNotificationHandler<TNotification>> _logger;

        protected DeleteNotificationHandler(ILogger<DeleteNotificationHandler<TNotification>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.DeleteStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Deleting model with key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DeleteEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Deleted model with key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DeleteError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error deleting model with key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
