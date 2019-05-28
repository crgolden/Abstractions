namespace Clarity.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;

    [ExcludeFromCodeCoverage]
    public abstract class CreateRequest<TEntity, TModel> : IRequest<(TModel, object[])>
        where TEntity : class
    {
        public readonly TModel Model;

        protected CreateRequest(TModel model)
        {
            Model = model;
        }
    }
}
