namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Shared;

    public abstract class UpdateNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : UpdateNotification<TModel>
    {
        private readonly ILogger<UpdateNotificationHandler<TNotification, TModel>> _logger;

        protected UpdateNotificationHandler(ILogger<UpdateNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.UpdateStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Updating model {@Model} at {@Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.UpdateEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Updated model {@Model} at {@Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.UpdateError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error updating model {@Model} at {@Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
