namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeCreateRangeRequestHandler : CreateRangeRequestHandler<CreateRangeRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeCreateRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
