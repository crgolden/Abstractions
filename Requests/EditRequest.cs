namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class EditRequest<TEntity, TModel> : IRequest
        where TEntity : class
    {
        public readonly TModel Model;

        protected EditRequest(TModel model)
        {
            Model = model;
        }
    }
}
