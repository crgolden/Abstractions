namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeUpdateRequestHandler : UpdateRequestHandler<UpdateRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeUpdateRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
