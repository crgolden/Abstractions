namespace crgolden.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeCreateRangeRequestHandler : CreateRangeRequestHandler<CreateRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeCreateRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
