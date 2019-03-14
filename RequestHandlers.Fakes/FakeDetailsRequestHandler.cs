namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeDetailsRequestHandler : DetailsRequestHandler<DetailsRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeDetailsRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
