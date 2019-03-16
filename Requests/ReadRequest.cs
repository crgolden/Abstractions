namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class ReadRequest<TEntity, TModel> : IRequest<TModel>
        where TEntity : class
    {
        public readonly object[] KeyValues;

        protected ReadRequest(object[] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
