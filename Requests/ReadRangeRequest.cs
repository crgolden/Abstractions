namespace Clarity.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;

    [ExcludeFromCodeCoverage]
    public abstract class ReadRangeRequest<TEntity, TModel> : IRequest<TModel[]>
        where TEntity : class
    {
        public object[][] KeyValues { get; set; }

        protected ReadRangeRequest(object[][] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
