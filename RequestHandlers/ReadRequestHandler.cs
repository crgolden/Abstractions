namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    public abstract class ReadRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel>
        where TRequest : ReadRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IMemoryCache Cache;

        protected ReadRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache)
        {
            Context = context;
            Mapper = mapper;
            Cache = cache;
        }

        public virtual async Task<TModel> Handle(TRequest request, CancellationToken token)
        {
            if (Cache.TryGetValue(request.KeyValues, out TModel model)) return model;
            var entity = await Context
                .FindAsync<TEntity>(request.KeyValues, token)
                .ConfigureAwait(false);
            model = Mapper.Map<TModel>(entity);
            using (var cacheEntry = Cache.CreateEntry(request.KeyValues))
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                cacheEntry.SetValue(model);
            }

            return model;
        }
    }
}
