namespace Clarity.Abstractions.Fakes
{
    using System.Collections.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    internal class FakeCreateRangeRequestHandler
        : CreateRangeRequestHandler<CreateRangeRequest<IEnumerable<object>, FakeEntity, object>, IEnumerable<object>, FakeEntity, object>
    {
        internal FakeCreateRangeRequestHandler(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
