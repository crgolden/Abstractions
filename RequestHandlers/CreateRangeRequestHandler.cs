namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class CreateRangeRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel[]>
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

        public virtual async Task<TModel[]> Handle(TRequest request, CancellationToken token)
        {
            var entities = Mapper.Map<TEntity[]>(request.Models);
            Context.Set<TEntity>().AddRange(entities);
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Mapper.Map<TModel[]>(entities);
        }
    }
}
