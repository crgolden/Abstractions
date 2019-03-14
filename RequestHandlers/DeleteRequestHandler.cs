namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class DeleteRequestHandler<TRequest, TEntity> : IRequestHandler<TRequest>
        where TRequest : DeleteRequest
        where TEntity : class
    {
        protected readonly DbContext Context;

        protected DeleteRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            var entity = await Context.FindAsync<TEntity>(request.KeyValues, token).ConfigureAwait(false);
            Context.Remove(entity);
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
