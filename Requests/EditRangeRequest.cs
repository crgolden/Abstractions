namespace Clarity.Abstractions
{
    using System.Collections.Generic;
    using MediatR;

    public abstract class EditRangeRequest<TEntity, TModel> : IRequest
        where TEntity : class
    {
        public readonly IEnumerable<TModel> Models;

        protected EditRangeRequest(IEnumerable<TModel> models)
        {
            Models = models;
        }
    }
}
