namespace Clarity.Abstractions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class DeleteRangeRequestHandler<TRequest, TEntity> : IRequestHandler<TRequest, object[][]>
        where TRequest : DeleteRangeRequest
        where TEntity : class
    {
        protected readonly DbContext Context;

        protected DeleteRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<object[][]> Handle(TRequest request, CancellationToken token)
        {
            var keyValuesList = new List<object[]>();
            foreach (var keyValues in request.KeyValues)
            {
                var entity = await Context.FindAsync<TEntity>(keyValues, token).ConfigureAwait(false);
                var entityEntry = Context.Entry(entity);
                entityEntry.State = EntityState.Deleted;
                keyValuesList.Add(keyValues);
                foreach (var collection in entityEntry.Collections.Select(x => x.CurrentValue))
                {
                    keyValuesList.AddRange(from object item in collection
                        select Context.Entry(item)
                        into itemEntry
                        select itemEntry.Metadata.FindPrimaryKey()
                            .Properties.Select(x => entityEntry.Property(x.Name).CurrentValue)
                            .ToArray());
                }
            }

            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return keyValuesList.ToArray();
        }
    }
}
