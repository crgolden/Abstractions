namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class CreateRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, TModel>
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

        public virtual async Task<TModel> Handle(TRequest request, CancellationToken token)
        {
            var entity = Mapper.Map<TEntity>(request.Model);
            Context.Add(entity);
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Mapper.Map<TModel>(entity);
        }
    }
}
