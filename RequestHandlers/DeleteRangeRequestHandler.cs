namespace Clarity.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    public abstract class DeleteRangeRequestHandler<TRequest, TEntity> : IRequestHandler<TRequest>
        where TRequest : DeleteRangeRequest
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMemoryCache Cache;

        protected DeleteRangeRequestHandler(DbContext context, IMemoryCache cache)
        {
            Context = context;
            Cache = cache;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            foreach (var keyValues in request.KeyValues)
            {
                var entity = await Context.FindAsync<TEntity>(keyValues, token).ConfigureAwait(false);
                var entityEntry = Context.Entry(entity);
                entityEntry.State = EntityState.Deleted;
                Cache.Remove(keyValues);
                foreach (var collection in entityEntry.Collections.Select(x => x.CurrentValue))
                {
                    foreach (var item in collection)
                    {
                        var itemEntry = Context.Entry(item);
                        var itemKeyValues = itemEntry.Metadata
                            .FindPrimaryKey()
                            .Properties
                            .Select(x => entityEntry.Property(x.Name).CurrentValue)
                            .ToArray();
                        Cache.Remove(itemKeyValues);
                    }
                }
            }

            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
