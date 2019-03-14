namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class ValidationRequest<TModel> : IRequest<bool>
    {
        public readonly TModel Model;

        protected ValidationRequest(TModel model)
        {
            Model = model;
        }
    }
}
