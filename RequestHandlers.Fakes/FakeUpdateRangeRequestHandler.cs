namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    internal class FakeUpdateRangeRequestHandler : UpdateRangeRequestHandler<UpdateRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeUpdateRangeRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache) : base(context, mapper, cache)
        {
        }
    }
}
