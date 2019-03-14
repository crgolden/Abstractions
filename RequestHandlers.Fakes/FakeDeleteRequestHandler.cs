namespace Clarity.Abstractions.Fakes
{
    using Microsoft.EntityFrameworkCore;

    internal class FakeDeleteRequestHandler : DeleteRequestHandler<DeleteRequest, FakeEntity>
    {
        internal FakeDeleteRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
