namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeListRequestHandler : ListRequestHandler<ListRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeListRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
