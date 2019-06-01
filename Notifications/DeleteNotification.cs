namespace crgolden.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Shared;

    [ExcludeFromCodeCoverage]

    public abstract class DeleteNotification : INotification
    {
        public EventIds EventId { get; set; }

        public object[] KeyValues { get; set; }

        public Exception Exception { get; set; }
    }
}
