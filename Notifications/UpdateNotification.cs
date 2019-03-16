namespace Clarity.Abstractions
{
    using System;
    using MediatR;
    using Shared;

    public abstract class UpdateNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public TModel Model { get; set; }

        public Exception Exception { get; set; }
    }
}
