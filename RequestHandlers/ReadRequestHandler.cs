namespace crgolden.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class ReadRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel>
        where TRequest : ReadRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected ReadRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<TModel> Handle(TRequest request, CancellationToken token)
        {
            
            var entity = await Context
                .FindAsync<TEntity>(request.KeyValues, token)
                .ConfigureAwait(false);
            return Mapper.Map<TModel>(entity);
        }
    }
}
