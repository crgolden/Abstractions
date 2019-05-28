namespace Clarity.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeUpdateRangeRequestHandler : UpdateRangeRequestHandler<UpdateRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeUpdateRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
