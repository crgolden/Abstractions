namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeReadRangeRequestHandler : ReadRangeRequestHandler<ReadRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeReadRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
