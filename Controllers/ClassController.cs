namespace Clarity.Abstractions.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Shared;

    [Route("v1/[controller]/[action]")]
    [ApiController]
    public abstract class ClassController<TEntity, TModel, TKey> : ControllerBase
        where TEntity : class
    {
        protected readonly IMediator Mediator;

        protected ClassController(IMediator mediator)
        {
            Mediator = mediator;
        }

        public abstract Task<IActionResult> List(DataSourceRequest request);

        protected virtual async Task<IActionResult> List<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : ListRequest<TEntity, TModel>
            where TNotification : ListNotification
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Request = request.Request;
                    notification.EventId = EventIds.ListStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Result = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.ListEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Result);
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.ListError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request);
                }
            }
        }

        public abstract Task<IActionResult> Read(TKey[] keyValues);

        protected virtual async Task<IActionResult> Read<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : ReadRequest<TEntity, TModel>
            where TNotification : ReadNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.KeyValues = request.KeyValues;
                    notification.EventId = EventIds.ReadStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Model = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    if (notification.Model == null)
                    {
                        notification.EventId = EventIds.ReadNotFound;
                        await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                        return NotFound(request.KeyValues);
                    }

                    notification.EventId = EventIds.ReadEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Model);
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.ReadError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.KeyValues);
                }
            }
        }

        public abstract Task<IActionResult> Update(TModel model);

        protected virtual async Task<IActionResult> Update<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : UpdateRequest<TEntity, TModel>
            where TNotification : UpdateNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Model = request.Model;
                    notification.EventId = EventIds.UpdateStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.UpdateEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return NoContent();
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.UpdateError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.Model);
                }
            }
        }

        public abstract Task<IActionResult> Create(TModel model);

        protected virtual async Task<IActionResult> Create<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : CreateRequest<TEntity, TModel>
            where TNotification : CreateNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Model = request.Model;
                    notification.EventId = EventIds.CreateStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Model = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.CreateEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Model);
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.CreateError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.Model);
                }
            }
        }

        public abstract Task<IActionResult> Delete(TKey[] keyValues);

        protected virtual async Task<IActionResult> Delete<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : DeleteRequest
            where TNotification : DeleteNotification
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
