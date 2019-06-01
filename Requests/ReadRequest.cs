namespace crgolden.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;

    [ExcludeFromCodeCoverage]
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
