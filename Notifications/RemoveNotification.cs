namespace Clarity.Abstractions
{
    using System;
    using System.Collections.Generic;
    using Core;
    using MediatR;

    public abstract class RemoveNotification<TKey> : INotification
    {
        public EventIds EventId { get; set; }

        public IEnumerable<string> FileNames { get; set; }

        public TKey[][] KeyValues { get; set; }

        public Exception Exception { get; set; }
    }
}
