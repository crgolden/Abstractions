namespace Clarity.Abstractions
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class CreateRangeRequest<TResponse, TEntity, TModel> : IRequest<TResponse>
        where TEntity : class
        where TResponse : IEnumerable<TModel>
    {
        public readonly IEnumerable<TModel> Models;

        protected CreateRangeRequest(IEnumerable<TModel> models)
        {
            Models = models;
        }
    }
}
