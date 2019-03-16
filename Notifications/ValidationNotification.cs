namespace Clarity.Abstractions
{
    using System;
    using MediatR;
    using Shared;

    public abstract class ValidateNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public TModel Model { get; set; }

        public bool Valid { get; set; } = true;

        public Exception Exception { get; set; }
    }
}
