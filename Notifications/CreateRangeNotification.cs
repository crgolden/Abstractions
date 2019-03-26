namespace Clarity.Abstractions
{
    using System;
    using MediatR;
    using Shared;

    public abstract class CreateRangeNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public TModel[] Models { get; set; }

        public Exception Exception { get; set; }
    }
}
