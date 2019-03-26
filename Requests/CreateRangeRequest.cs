namespace Clarity.Abstractions
{
    using MediatR;

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
