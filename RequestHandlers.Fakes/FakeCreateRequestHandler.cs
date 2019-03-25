namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    internal class FakeCreateRequestHandler : CreateRequestHandler<CreateRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeCreateRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache) : base(context, mapper, cache)
        {
        }
    }
}
