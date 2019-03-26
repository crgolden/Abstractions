namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeCreateRequestHandler : CreateRequestHandler<CreateRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeCreateRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
