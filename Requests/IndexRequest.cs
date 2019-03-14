namespace Clarity.Abstractions
{
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public abstract class IndexRequest<TEntity, TModel> : IRequest<DataSourceResult>
        where TEntity : class
    {
        public readonly ModelStateDictionary ModelState;

        public readonly DataSourceRequest Request;

        protected IndexRequest(ModelStateDictionary modelState, DataSourceRequest request)
        {
            ModelState = modelState;
            Request = request;
        }
    }
}
