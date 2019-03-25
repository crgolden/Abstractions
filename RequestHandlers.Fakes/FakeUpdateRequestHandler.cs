namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    internal class FakeUpdateRequestHandler : UpdateRequestHandler<UpdateRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeUpdateRequestHandler(DbContext context, IMapper mapper, IMemoryCache cache) : base(context, mapper, cache)
        {
        }
    }
}
