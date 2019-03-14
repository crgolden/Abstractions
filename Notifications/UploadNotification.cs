namespace Clarity.Abstractions
{
    using System;
    using System.Collections.Generic;
    using Core;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public abstract class UploadNotification<TModel> : INotification
    {
        public EventIds EventId { get; set; }

        public IFormFileCollection Files { get; set; }

        public IEnumerable<TModel> Models { get; set; }

        public Exception Exception { get; set; }
    }
}
