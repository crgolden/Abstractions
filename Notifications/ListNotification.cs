namespace crgolden.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using MediatR;
    using Microsoft.AspNet.OData.Query;
    using Shared;

    [ExcludeFromCodeCoverage]

    public abstract class ListNotification : INotification
    {
        public EventIds EventId { get; set; }

        public ODataQueryOptions Options { get; set; }

        public IQueryable Result {get; set; }

        public Exception Exception { get; set; }
    }
}
