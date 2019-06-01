namespace crgolden.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeUpdateRequestHandler : UpdateRequestHandler<UpdateRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeUpdateRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
