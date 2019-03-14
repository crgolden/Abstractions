namespace Clarity.Abstractions
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class CreateRangeRequestHandler<TRequest, TResponse, TEntity, TModel> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRangeRequest<TResponse, TEntity, TModel>
        where TResponse : IEnumerable<TModel>
        where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly IMapper Mapper;

        protected CreateRangeRequestHandler(DbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken token)
        {
            var entities = Mapper.Map<IEnumerable<TEntity>>(request.Models);
            Context.AddRange(entities);
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Mapper.Map<TResponse>(entities);
        }
    }
}
