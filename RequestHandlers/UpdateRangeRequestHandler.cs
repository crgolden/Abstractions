namespace crgolden.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class UpdateRangeRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, object[][]>
        where TRequest : UpdateRangeRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected UpdateRangeRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<object[][]> Handle(TRequest request, CancellationToken token)
        {
            var keyValues = new object[request.Models.Length][];
            for (var i = 0; i < request.Models.Length; i++)
            {
                var entity = Mapper.Map<TEntity>(request.Models[i]);
                var entityEntry = Context.Entry(entity);
                entityEntry.State = EntityState.Modified;
                keyValues[i] = entityEntry.Metadata
                    .FindPrimaryKey()
                    .Properties
                    .Select(x => entityEntry.Property(x.Name).CurrentValue)
                    .ToArray();
            }
            
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return keyValues;
        }
    }
}
