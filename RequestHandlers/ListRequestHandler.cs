namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class ListRequestHandler<TRequest, TEntity, TModel> : IRequestHandler<TRequest, DataSourceResult>
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

        public virtual async Task<DataSourceResult> Handle(TRequest request, CancellationToken token)
        {
            var entities = Context.Set<TEntity>();
            return await Mapper
                .ProjectTo<TModel>(entities)
                .ToDataSourceResultAsync(request.Request, request.ModelState)
                .ConfigureAwait(false);
        }
    }
}
