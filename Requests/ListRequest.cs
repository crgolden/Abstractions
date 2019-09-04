namespace crgolden.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using MediatR;
    using Microsoft.AspNet.OData.Query;

    [ExcludeFromCodeCoverage]
    public abstract class ListRequest<TEntity, TModel> : IRequest<IQueryable<TModel>>
        where TEntity : class
    {
        public readonly ODataQueryOptions<TModel> Options;

        protected ListRequest(ODataQueryOptions<TModel> options)
        {
            Options = options;
        }
    }
}
