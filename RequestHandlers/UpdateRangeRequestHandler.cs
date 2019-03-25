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

    public abstract class UpdateRangeRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest>
        where TRequest : UpdateRangeRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IMemoryCache Cache;

        protected UpdateRangeRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache)
        {
            Context = context;
            Mapper = mapper;
            Cache = cache;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            foreach (var model in request.Models)
            {
                var entity = Mapper.Map<TEntity>(model);
                var entityEntry = Context.Entry(entity);
                entityEntry.State = EntityState.Modified;
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
            }
            
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
