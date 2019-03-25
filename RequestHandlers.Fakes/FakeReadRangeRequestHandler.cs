namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    internal class FakeReadRangeRequestHandler : ReadRangeRequestHandler<ReadRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeReadRangeRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache) : base(context, mapper, cache)
        {
        }
    }
}
