namespace Clarity.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeReadRangeRequestHandler : ReadRangeRequestHandler<ReadRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeReadRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
