namespace Clarity.Abstractions.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Shared;

    [Produces("application/json")]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    public abstract class ClassController<TEntity, TModel, TKey> : ControllerBase
        where TEntity : class
    {
        protected readonly string TypeName;
        protected readonly IMediator Mediator;
        protected readonly IMemoryCache Cache;
        protected readonly DateTimeOffset? AbsoluteExpiration;
        protected readonly TimeSpan? AbsoluteExpirationRelativeToNow;
        protected readonly TimeSpan? SlidingExpiration;

        protected ClassController(IMediator mediator, IMemoryCache cache, IOptions<CacheOptions> cacheOptions)
        {
            TypeName = typeof(TModel).Name;
            Mediator = mediator;
            Cache = cache;
            AbsoluteExpiration = cacheOptions.Value?.AbsoluteExpiration;
            AbsoluteExpirationRelativeToNow = cacheOptions.Value?.AbsoluteExpirationRelativeToNow;
            SlidingExpiration = cacheOptions.Value?.SlidingExpiration;
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

                    var key = JsonConvert.SerializeObject(request.Request);
                    if (Cache.TryGetValue($"{TypeName}: {key}", out DataSourceResult value))
                    {
                        notification.Result = value;
                    }
                    else
                    {
                        notification.Result = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                        using (var entry = Cache.CreateEntry($"{TypeName}: {key}"))
                        {
                            entry.SlidingExpiration = SlidingExpiration;
                            entry.AbsoluteExpiration = AbsoluteExpiration;
                            entry.AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNow;
                            entry.SetValue(notification.Result);
                        }
                    }

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

                    var key = JsonConvert.SerializeObject(request.KeyValues);
                    if (Cache.TryGetValue($"{TypeName}: {key}", out TModel value))
                    {
                        notification.Model = value;
                    }
                    else
                    {
                        notification.Model = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                        if (notification.Model != null) SetCache(request.KeyValues, notification.Model);
                    }

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

                    var keyValues = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    SetCache(keyValues, notification.Model);

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

                    var (model, keyValues) = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    SetCache(keyValues, model);

                    notification.Model = model;
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

                    var keyValuesArray = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    foreach (var keyValues in keyValuesArray)
                    {
                        var key = JsonConvert.SerializeObject(keyValues);
                        Cache.Remove($"{TypeName}: {key}");
                    }

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

        protected virtual void SetCache(object[] keyValues, TModel model)
        {
            var key = JsonConvert.SerializeObject(keyValues);
            using (var entry = Cache.CreateEntry($"{TypeName}: {key}"))
            {
                entry.SlidingExpiration = SlidingExpiration;
                entry.AbsoluteExpiration = AbsoluteExpiration;
                entry.AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNow;
                entry.SetValue(model);
            }
        }
    }
}
