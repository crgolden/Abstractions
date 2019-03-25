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

    public abstract class CreateRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel>
        where TRequest : CreateRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IMemoryCache Cache;

        protected CreateRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache)
        {
            Context = context;
            Mapper = mapper;
            Cache = cache;
        }

        public virtual async Task<TModel> Handle(TRequest request, CancellationToken token)
        {
            var entity = Mapper.Map<TEntity>(request.Model);
            var entityEntry = Context.Entry(entity);
            entityEntry.State = EntityState.Added;
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            var model = Mapper.Map<TModel>(entity);
            var keyValues = entityEntry.Metadata
                .FindPrimaryKey()
                .Properties
                .Select(x => entityEntry.Property(x.Name).CurrentValue)
                .ToArray();
            using (var cacheEntry = Cache.CreateEntry(keyValues))
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                cacheEntry.SetValue(model);
            }

            return model;
        }
    }
}
