namespace Clarity.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;

    [ExcludeFromCodeCoverage]
    public abstract class CreateRangeRequest<TEntity, TModel> : IRequest<(TModel[], object[][])>
        where TEntity : class
    {
        public readonly TModel[] Models;

        protected CreateRangeRequest(TModel[] models)
        {
            Models = models;
        }
    }
}
