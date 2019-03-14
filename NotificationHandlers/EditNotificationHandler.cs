namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class EditNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : EditNotification<TModel>
    {
        private readonly ILogger<EditNotificationHandler<TNotification, TModel>> _logger;

        protected EditNotificationHandler(ILogger<EditNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.EditStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Editing model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.EditEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Edited model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.EditError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error editing model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
