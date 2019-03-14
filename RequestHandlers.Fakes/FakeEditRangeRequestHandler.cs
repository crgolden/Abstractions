namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeEditRangeRequestHandler : EditRangeRequestHandler<EditRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeEditRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
