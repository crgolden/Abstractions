namespace Clarity.Abstractions
{
    using System;
    using Core;
    using Kendo.Mvc.UI;
    using MediatR;

    public abstract class IndexNotification : INotification
    {
        public EventIds EventId { get; set; }

        public DataSourceRequest Request { get; set; }

        public DataSourceResult Result {get; set; }

        public Exception Exception { get; set; }
    }
}
