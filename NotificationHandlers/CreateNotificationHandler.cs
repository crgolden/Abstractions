namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class CreateNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : CreateNotification<TModel>
    {
        private readonly ILogger<CreateNotificationHandler<TNotification, TModel>> _logger;

        protected CreateNotificationHandler(ILogger<CreateNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.CreateStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Creating model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.CreateEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Created model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.CreateError:
                    _logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error creating model {Model} at {Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
