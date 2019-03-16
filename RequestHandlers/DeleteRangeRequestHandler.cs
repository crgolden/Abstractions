namespace Clarity.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class DeleteRangeRequestHandler<TRequest, TEntity> : IRequestHandler<TRequest>
        where TRequest : DeleteRangeRequest
        where TEntity : class
    {
        protected readonly DbContext Context;

        protected DeleteRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            var tasks = request.KeyValues.Select(x => Context.FindAsync<TEntity>(x, token));
            var entities = await Task.WhenAll(tasks).ConfigureAwait(false);
            Context.Set<TEntity>().RemoveRange(entities);
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
