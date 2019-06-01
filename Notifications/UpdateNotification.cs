namespace crgolden.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Shared;

    [ExcludeFromCodeCoverage]
    public abstract class UpdateNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public TModel Model { get; set; }

        public Exception Exception { get; set; }
    }
}
