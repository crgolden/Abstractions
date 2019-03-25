namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    public abstract class ListRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, DataSourceResult>
        where TRequest : ListRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IMemoryCache Cache;

        protected ListRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache)
        {
            Context = context;
            Mapper = mapper;
            Cache = cache;
        }

        public virtual async Task<DataSourceResult> Handle(TRequest request, CancellationToken token)
        {
            if (Cache.TryGetValue(request.Request, out DataSourceResult result)) return result;
            var entities = Context.Set<TEntity>().AsNoTracking();
            result = await Mapper
                .ProjectTo<TModel>(entities)
                .ToDataSourceResultAsync(request.Request, request.ModelState)
                .ConfigureAwait(false);
            using (var cacheEntry = Cache.CreateEntry(request.Request))
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                cacheEntry.SetValue(result);
            }

            return result;
        }
    }
}
