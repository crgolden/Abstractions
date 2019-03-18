namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Shared;

    public abstract class ReadNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : ReadNotification<TModel>
    {
        private readonly ILogger<ReadNotificationHandler<TNotification, TModel>> _logger;

        protected ReadNotificationHandler(ILogger<ReadNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.ReadStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Details requested for key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.ReadNotFound:
                    _logger.LogWarning(
                        eventId: eventId,
                        message: "Details not found for key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.ReadEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Details found for model {@Model} at {@Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.ReadError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error finding details for key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
