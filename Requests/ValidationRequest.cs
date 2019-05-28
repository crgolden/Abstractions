namespace Clarity.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;

    [ExcludeFromCodeCoverage]
    public abstract class ValidationRequest<TModel> : IRequest<bool>
    {
        public readonly TModel Model;

        protected ValidationRequest(TModel model)
        {
            Model = model;
        }
    }
}
