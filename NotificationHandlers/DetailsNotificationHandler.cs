namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class DetailsNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : DetailsNotification<TModel>
    {
        private readonly ILogger<DetailsNotificationHandler<TNotification, TModel>> _logger;

        protected DetailsNotificationHandler(ILogger<DetailsNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.DetailsStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Details requested for key value(s) {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DetailsNotFound:
                    _logger.LogWarning(
                        eventId: eventId,
                        message: "Details not found for key value(s) {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.DetailsEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Details found for model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.DetailsError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error finding details for key value(s) {KeyValues} at {Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
