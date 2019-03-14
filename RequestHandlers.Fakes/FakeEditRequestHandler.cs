namespace Clarity.Abstractions.Fakes
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeEditRequestHandler : EditRequestHandler<EditRequest<FakeEntity, object>, FakeEntity, object>
    {
        internal FakeEditRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
