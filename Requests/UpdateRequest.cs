namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class UpdateRequest<TEntity, TModel> : IRequest
        where TEntity : class
    {
        public readonly TModel Model;

        protected UpdateRequest(TModel model)
        {
            Model = model;
        }
    }
}
