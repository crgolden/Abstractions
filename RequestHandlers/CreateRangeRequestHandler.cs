namespace Clarity.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class CreateRangeRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, (TModel[], object[][])>
        where TRequest : CreateRangeRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected CreateRangeRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<(TModel[], object[][])> Handle(TRequest request, CancellationToken token)
        {
            var models = new TModel[request.Models.Length];
            var keyValues = new object[request.Models.Length][];
            for (var i = 0; i < request.Models.Length; i++)
            {
                var entity = Mapper.Map<TEntity>(request.Models[i]);
                var entityEntry = Context.Entry(entity);
                entityEntry.State = EntityState.Added;
                models[i] = Mapper.Map<TModel>(entity);
                keyValues[i] = entityEntry.Metadata
                    .FindPrimaryKey()
                    .Properties
                    .Select(y => entityEntry.Property(y.Name).CurrentValue)
                    .ToArray();
            }

            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return (models, keyValues);
        }
    }
}
