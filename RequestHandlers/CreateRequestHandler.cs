namespace crgolden.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class CreateRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, (TModel, object[])>
        where TRequest : CreateRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected CreateRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<(TModel, object[])> Handle(TRequest request, CancellationToken token)
        {
            var entity = Mapper.Map<TEntity>(request.Model);
            var entityEntry = Context.Entry(entity);
            entityEntry.State = EntityState.Added;
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return (Mapper.Map<TModel>(entity), entityEntry.Metadata
                .FindPrimaryKey()
                .Properties
                .Select(x => entityEntry.Property(x.Name).CurrentValue)
                .ToArray());
        }
    }
}
