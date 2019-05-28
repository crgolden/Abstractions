namespace Clarity.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    [ExcludeFromCodeCoverage]
    public abstract class ListRequest<TEntity, TModel> : IRequest<DataSourceResult>
        where TEntity : class
    {
        public readonly ModelStateDictionary ModelState;

        public readonly DataSourceRequest Request;

        protected ListRequest(ModelStateDictionary modelState, DataSourceRequest request)
        {
            ModelState = modelState;
            Request = request;
        }
    }
}
