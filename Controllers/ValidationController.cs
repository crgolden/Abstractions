namespace crgolden.Abstractions.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNetCore.Mvc;
    using Shared;

    [Produces("application/json")]
    public abstract class ValidationController<TModel> : ODataController
    {
        protected readonly IMediator Mediator;

        protected ValidationController(IMediator mediator)
        {
            Mediator = mediator;
        }

        public abstract Task<IActionResult> Validate(TModel model);

        protected virtual async Task<IActionResult> Validate<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : ValidationRequest<TModel>
            where TNotification : ValidateNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Model = request.Model;
                    notification.EventId = EventIds.ValidateStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Valid = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.ValidateEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Valid);
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.ValidateError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return Ok(notification.Valid);
                }
            }
        }
    }
}
