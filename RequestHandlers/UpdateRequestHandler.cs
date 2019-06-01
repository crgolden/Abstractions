namespace crgolden.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class UpdateRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, object[]>
        where TRequest : UpdateRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected UpdateRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<object[]> Handle(TRequest request, CancellationToken token)
        {
            var entity = Mapper.Map<TEntity>(request.Model);
            var entityEntry = Context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return entityEntry.Metadata
                .FindPrimaryKey()
                .Properties
                .Select(y => entityEntry.Property(y.Name).CurrentValue)
                .ToArray();
        }
    }
}
