namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class EditRangeNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : EditRangeNotification<TModel>
    {
        private readonly ILogger<EditRangeNotificationHandler<TNotification, TModel>> _logger;

        protected EditRangeNotificationHandler(ILogger<EditRangeNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.EditRangeStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Editing models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.EditRangeEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Edited models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.EditRangeError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error editing models {Models} at {Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
