namespace Clarity.Abstractions.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public abstract class Controller<TEntity, TModel, TKey> : ControllerBase
        where TEntity : class
    {
        protected readonly IMediator Mediator;

        protected Controller(IMediator mediator)
        {
            Mediator = mediator;
        }

        public abstract Task<IActionResult> Index(DataSourceRequest request);

        protected virtual async Task<IActionResult> Index<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : IndexRequest<TEntity, TModel>
            where TNotification : IndexNotification
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Request = request.Request;
                    notification.EventId = EventIds.IndexStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Result = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.IndexEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Result);
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.IndexError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request);
                }
            }
        }

        public abstract Task<IActionResult> Details(TKey[] keyValues);

        protected virtual async Task<IActionResult> Details<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : DetailsRequest<TEntity, TModel>
            where TNotification : DetailsNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.KeyValues = request.KeyValues;
                    notification.EventId = EventIds.DetailsStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Model = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    if (notification.Model == null)
                    {
                        notification.EventId = EventIds.DetailsNotFound;
                        await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                        return NotFound(request.KeyValues);
                    }

                    notification.EventId = EventIds.DetailsEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Model);
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.DetailsError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.KeyValues);
                }
            }
        }

        public abstract Task<IActionResult> Edit(TModel model);

        protected virtual async Task<IActionResult> Edit<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : EditRequest<TEntity, TModel>
            where TNotification : EditNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Model = request.Model;
                    notification.EventId = EventIds.EditStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.EditEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return NoContent();
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.EditError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.Model);
                }
            }
        }

        public abstract Task<IActionResult> EditRange(IEnumerable<TModel> models);

        protected virtual async Task<IActionResult> EditRange<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : EditRangeRequest<TEntity, TModel>
            where TNotification : EditRangeNotification<TModel>
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Models = request.Models;
                    notification.EventId = EventIds.EditRangeStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.EditRangeEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return NoContent();
                }
                catch (Exception e)
                {
                    tokenSource.Cancel();
                    notification.Exception = e;
                    notification.EventId = EventIds.EditRangeError;
                    await Mediator.Publish(notification, CancellationToken.None).ConfigureAwait(false);
                    return BadRequest(request.Models);
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

        public abstract Task<IActionResult> CreateRange(IEnumerable<TModel> models);

        protected virtual async Task<IActionResult> CreateRange<TRequest, TNotification>(
            TRequest request,
            TNotification notification)
            where TRequest : CreateRangeRequest<IEnumerable<TModel>, TEntity, TModel>
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
