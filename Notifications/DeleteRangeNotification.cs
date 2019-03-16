namespace Clarity.Abstractions
{
    using System;
    using MediatR;
    using Shared;

    public abstract class DeleteRangeNotification : INotification
    {
        public EventIds EventId { get; set; }

        public object[][] KeyValues { get; set; }

        public Exception Exception { get; set; }
    }
}
