namespace Clarity.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Kendo.Mvc.UI;
    using MediatR;
    using Shared;

    [ExcludeFromCodeCoverage]

    public abstract class ListNotification : INotification
    {
        public EventIds EventId { get; set; }

        public DataSourceRequest Request { get; set; }

        public DataSourceResult Result {get; set; }

        public Exception Exception { get; set; }
    }
}
