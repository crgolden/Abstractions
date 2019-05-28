namespace Clarity.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeCreateRequestHandler : CreateRequestHandler<CreateRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeCreateRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
