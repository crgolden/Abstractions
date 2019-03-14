namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class DetailsRequest<TEntity, TModel> : IRequest<TModel>
        where TEntity : class
    {
        public readonly object[] KeyValues;

        protected DetailsRequest(object[] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
