namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Shared;

    public abstract class ValidationRequestHandler<TRequest, TModel> : IRequestHandler<TRequest, bool>
        where TRequest : ValidationRequest<TModel>
    {
        private readonly IValidationService<TModel> _validationService;

        protected ValidationRequestHandler(IValidationService<TModel> validationService)
        {
            _validationService = validationService;
        }

        public virtual Task<bool> Handle(TRequest request, CancellationToken token)
        {
            return _validationService.ValidateAsync(request.Model, token);
        }
    }
}
