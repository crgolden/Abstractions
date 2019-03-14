namespace Clarity.Abstractions
{
    using System;
    using Core;
    using MediatR;

    public abstract class DetailsNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public object[] KeyValues { get; set; }

        public TModel Model { get; set; }

        public Exception Exception { get; set; }
    }
}
