namespace Clarity.Abstractions
{
    using System;
    using System.Collections.Generic;
    using Core;
    using MediatR;

    public abstract class EditRangeNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public IEnumerable<TModel> Models { get; set; }

        public Exception Exception { get; set; }
    }
}
