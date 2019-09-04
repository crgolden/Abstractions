namespace crgolden.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class ListRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, IQueryable<TModel>>
        where TRequest : ListRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected ListRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual Task<IQueryable<TModel>> Handle(TRequest request, CancellationToken token)
        {
            var entities = Context.Set<TEntity>().AsNoTracking();
            var query = request.Options.ApplyTo(entities);
            var result = Mapper.ProjectTo<TModel>(query);
            return Task.FromResult(result);
        }
    }
}
