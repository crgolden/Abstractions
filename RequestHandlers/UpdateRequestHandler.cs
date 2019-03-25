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

    public abstract class UpdateRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest>
        where TRequest : UpdateRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;
        protected readonly IMemoryCache Cache;

        protected UpdateRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache)
        {
            Context = context;
            Mapper = mapper;
            Cache = cache;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            var entity = Mapper.Map<TEntity>(request.Model);
            var entityEntry = Context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            var keyValues = entityEntry.Metadata
                .FindPrimaryKey()
                .Properties
                .Select(y => entityEntry.Property(y.Name).CurrentValue)
                .ToArray();
            using (var cacheEntry = Cache.CreateEntry(keyValues))
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                cacheEntry.SetValue(request.Model);
            }

            return Unit.Value;
        }
    }
}
