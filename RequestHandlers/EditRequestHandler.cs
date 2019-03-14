namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class EditRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest>
        where TRequest : EditRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected EditRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            var entity = Mapper.Map<TEntity>(request.Model);
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
