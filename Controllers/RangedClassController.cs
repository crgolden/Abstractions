namespace crgolden.Abstractions.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Shared;

    public abstract class RangedClassController<TEntity, TModel, TKey> : ControllerBase<TEntity, TModel, TKey>
        where TEntity : class
    {
        protected RangedClassController(IMediator mediator, IMemoryCache cache, IOptions<CacheOptions> cacheOptions)
            : base(mediator, cache, cacheOptions)
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

                    var keyValuesList = new List<object[]>();
                    var modelsList = new List<TModel>();
                    foreach (var keyValues in notification.KeyValues)
                    {
                        var key = JsonConvert.SerializeObject(keyValues);
                        if (Cache.TryGetValue($"{TypeName}: {key}", out TModel value))
                        {
                            modelsList.Add(value);
                        }
                        else
                        {
                            keyValuesList.Add(keyValues);
                        }
                    }

                    request.KeyValues = keyValuesList.ToArray();
                    var models = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.Models = modelsList.Union(models).ToArray();
                    for (var i = 0; i < models.Length; i++) SetCache(request.KeyValues[i], models[i]);

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

                    var keyValues = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    for (var i = 0; i < keyValues.Length; i++) SetCache(keyValues[i], notification.Models[i]);

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

                    var (models, keyValues) = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    for (var i = 0; i < keyValues.Length; i++) SetCache(keyValues[i], models[i]);

                    notification.Models = models;
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
    }
}
