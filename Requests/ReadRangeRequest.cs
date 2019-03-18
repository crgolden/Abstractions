﻿namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class ReadRangeRequest<TEntity, TModel> : IRequest<TModel[]>
        where TEntity : class
    {
        public readonly object[][] KeyValues;

        protected ReadRangeRequest(object[][] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}