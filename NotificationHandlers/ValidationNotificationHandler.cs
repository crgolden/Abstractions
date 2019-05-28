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
    public abstract class ValidateNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : ValidateNotification<TModel>
    {
        protected readonly ILogger<ValidateNotificationHandler<TNotification, TModel>> Logger;

        protected ValidateNotificationHandler(ILogger<ValidateNotificationHandler<TNotification, TModel>> logger)
        {
            Logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.ValidateStart:
                    Logger.LogInformation(
                        eventId: eventId,
                        message: "Validating model {@Model} at {@Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.ValidateEnd:
                    Logger.LogInformation(
                        eventId: eventId,
                        message: "Validated model {@Model} at {@Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
                case EventIds.ValidateError:
                    Logger.LogError(
                        eventId: eventId,
                        exception: notification.Exception,
                        message: "Error validating model {@Model} at {@Time}",
                        args: new object[] { notification.Model, DateTime.UtcNow });
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
