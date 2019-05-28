﻿namespace Clarity.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MediatR;
    using Shared;

    [ExcludeFromCodeCoverage]
    public abstract class UpdateRangeNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public TModel[] Models { get; set; }

        public Exception Exception { get; set; }
    }
}
