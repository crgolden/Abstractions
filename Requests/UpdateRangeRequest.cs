namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class UpdateRangeRequest<TEntity, TModel> : IRequest
        where TEntity : class
    {
        public readonly TModel[] Models;

        protected UpdateRangeRequest(TModel[] models)
        {
            Models = models;
        }
    }
}
