namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeIndexRequestHandler : IndexRequestHandler<IndexRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeIndexRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
