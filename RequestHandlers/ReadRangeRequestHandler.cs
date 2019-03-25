namespace Clarity.Abstractions
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    public abstract class ReadRangeRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel[]>
        where TRequest : ReadRangeRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IMemoryCache Cache;

        protected ReadRangeRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache)
        {
            Context = context;
            Mapper = mapper;
            Cache = cache;
        }

        public virtual async Task<TModel[]> Handle(TRequest request, CancellationToken token)
        {
            return await Task.WhenAll(request.KeyValues.Select(async keyValues =>
            {
                if (Cache.TryGetValue(keyValues, out TModel model)) return model;
                var entity = await Context.FindAsync<TEntity>(keyValues, token).ConfigureAwait(false);
                model = Mapper.Map<TModel>(entity);
                using (var cacheEntry = Cache.CreateEntry(keyValues))
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                    cacheEntry.SetValue(model);
                }

                return model;
            })).ConfigureAwait(false);
        }
    }
}
