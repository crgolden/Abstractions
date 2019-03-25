namespace Clarity.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    public abstract class DeleteRequestHandler<TRequest, TEntity> : IRequestHandler<TRequest>
        where TRequest : DeleteRequest
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMemoryCache Cache;

        protected DeleteRequestHandler(DbContext context, IMemoryCache cache)
        {
            Context = context;
            Cache = cache;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            var entity = await Context.FindAsync<TEntity>(request.KeyValues, token).ConfigureAwait(false);
            var entityEntry = Context.Entry(entity);
            entityEntry.State = EntityState.Deleted;
            Cache.Remove(request.KeyValues);
            foreach (var collection in entityEntry.Collections.Select(x => x.CurrentValue))
            {
                foreach (var item in collection)
                {
                    var itemEntry = Context.Entry(item);
                    var keyValues = itemEntry.Metadata
                        .FindPrimaryKey()
                        .Properties
                        .Select(x => entityEntry.Property(x.Name).CurrentValue)
                        .ToArray();
                    Cache.Remove(keyValues);
                }
            }

            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
