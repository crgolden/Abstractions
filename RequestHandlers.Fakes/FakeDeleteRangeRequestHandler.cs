namespace Clarity.Abstractions.Fakes
{
    using Microsoft.EntityFrameworkCore;

    internal class FakeDeleteRangeRequestHandler : DeleteRangeRequestHandler<DeleteRangeRequest, FakeEntity>
    {
        internal FakeDeleteRangeRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
