namespace crgolden.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeReadRequestHandler : ReadRequestHandler<ReadRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeReadRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
