namespace Clarity.Abstractions.Fakes
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    internal class FakeDeleteRequestHandler : DeleteRequestHandler<DeleteRequest, FakeEntity>
    {
        internal FakeDeleteRequestHandler(DbContext context, IMemoryCache cache) : base(context, cache)
        {
        }
    }
}
