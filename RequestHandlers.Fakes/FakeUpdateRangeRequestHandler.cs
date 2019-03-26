namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeUpdateRangeRequestHandler : UpdateRangeRequestHandler<UpdateRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeUpdateRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
