namespace Clarity.Abstractions.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Shared;

    public abstract class RangedClassController<TEntity, TModel, TKey> : ClassController<TEntity, TModel, TKey>
        where TEntity : class
    {
        protected RangedClassController(IMediator mediator) : base(mediator)
        {
        }

        public abstract Task<IActionResult> ReadRange(TKey[][] keyValues);

        protected virtual async Task<IActionResult> ReadRange<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : ReadRangeRequest<TEntity, TModel>
            where TNotification : ReadRangeNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.KeyValues = request.KeyValues;
                    notification.EventId = EventIds.ReadRangeStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Models = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.ReadRangeEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Models);
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.ReadRangeError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.KeyValues);
                }
            }
        }

        public abstract Task<IActionResult> UpdateRange(TModel[] models);

        protected virtual async Task<IActionResult> UpdateRange<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : UpdateRangeRequest<TEntity, TModel>
            where TNotification : UpdateRangeNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Models = request.Models;
                    notification.EventId = EventIds.UpdateRangeStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.UpdateRangeEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return NoContent();
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.UpdateRangeError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.Models);
                }
            }
        }

        public abstract Task<IActionResult> CreateRange(TModel[] models);

        protected virtual async Task<IActionResult> CreateRange<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : CreateRangeRequest<TEntity, TModel>
            where TNotification : CreateRangeNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Models = request.Models;
                    notification.EventId = EventIds.CreateRangeStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Models = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.CreateRangeEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Models);
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.CreateRangeError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.Models);
                }
            }
        }

        public abstract Task<IActionResult> DeleteRange(TKey[][] keyValues);

        protected virtual async Task<IActionResult> DeleteRange<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : DeleteRangeRequest
            where TNotification : DeleteRangeNotification
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.KeyValues = request.KeyValues;
                    notification.EventId = EventIds.DeleteStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    
                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.DeleteEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return NoContent();
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.DeleteError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.KeyValues);
                }
            }
        }
    }
}
