﻿namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class UpdateRangeRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest>
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

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken token)
        {
            var entities = Mapper.Map<TEntity[]>(request.Models);
            foreach (var entity in entities) Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}