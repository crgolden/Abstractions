namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeReadRequestHandler : ReadRequestHandler<ReadRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeReadRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
