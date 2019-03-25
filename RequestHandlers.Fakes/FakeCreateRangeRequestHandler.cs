namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    internal class FakeCreateRangeRequestHandler : CreateRangeRequestHandler<CreateRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeCreateRangeRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache) : base(context, mapper, cache)
        {
        }
    }
}
