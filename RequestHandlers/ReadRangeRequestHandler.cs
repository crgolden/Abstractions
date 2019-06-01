namespace crgolden.Abstractions
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class ReadRangeRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel[]>
        where TRequest : ReadRangeRequest<TEntity, TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected ReadRangeRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<TModel[]> Handle(TRequest request, CancellationToken token)
        {
            var tasks = request.KeyValues.Select(keyValues => Context.FindAsync<TEntity>(keyValues, token));
            var entities = await Task.WhenAll(tasks).ConfigureAwait(false);
            return Mapper.Map<TModel[]>(entities);
        }
    }
}
