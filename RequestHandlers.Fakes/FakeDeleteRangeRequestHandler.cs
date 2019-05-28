namespace Clarity.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeDeleteRangeRequestHandler : DeleteRangeRequestHandler<DeleteRangeRequest, FakeEntity>
    {
        internal FakeDeleteRangeRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
