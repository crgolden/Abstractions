namespace crgolden.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Shared;

    [ExcludeFromCodeCoverage]
    public abstract class ValidateNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public TModel Model { get; set; }

        public bool Valid { get; set; } = true;

        public Exception Exception { get; set; }
    }
}
