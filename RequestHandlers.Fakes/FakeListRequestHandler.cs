namespace crgolden.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeListRequestHandler : ListRequestHandler<ListRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeListRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
