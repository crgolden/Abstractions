namespace Clarity.Abstractions.Fakes
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    internal class FakeDeleteRangeRequestHandler : DeleteRangeRequestHandler<DeleteRangeRequest, FakeEntity>
    {
        internal FakeDeleteRangeRequestHandler(DbContext context, IMemoryCache cache) : base(context, cache)
        {
        }
    }
}
