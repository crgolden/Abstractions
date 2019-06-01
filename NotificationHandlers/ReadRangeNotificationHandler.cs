namespace crgolden.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Shared;

    [ExcludeFromCodeCoverage]
    public abstract class ReadRangeNotificationHandler<TNotification, TModel> : INotificationHandler<TNotification>
        where TNotification : ReadRangeNotification<TModel>
    {
        private readonly ILogger<ReadRangeNotificationHandler<TNotification, TModel>> _logger;

        protected ReadRangeNotificationHandler(ILogger<ReadRangeNotificationHandler<TNotification, TModel>> logger)
        {
            _logger = logger;
        }

        public virtual Task Handle(TNotification notification, CancellationToken token)
        {
            var eventId = new EventId((int)notification.EventId, $"{notification.EventId}");
            switch (notification.EventId)
            {
                case EventIds.ReadRangeStart:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Details requested for key value(s) {@KeyValues} at {@Time}",
                        args: new object[] { notification.KeyValues, DateTime.UtcNow });
                    break;
                case EventIds.ReadRangeEnd:
                    _logger.LogInformation(
                        eventId: eventId,
                        message: "Details found for model(s) {@Models} at {@Time}",
                        args: new object[] { notification.Models, DateTime.UtcNow });
                    break;
                case EventIds.ReadRangeError:
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
