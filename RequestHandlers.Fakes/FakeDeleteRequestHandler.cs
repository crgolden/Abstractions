namespace crgolden.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;

    [ExcludeFromCodeCoverage]
    internal class FakeDeleteRequestHandler : DeleteRequestHandler<DeleteRequest, FakeEntity>
    {
        internal FakeDeleteRequestHandler(DbContext context) : base(context)
        {
        }
    }
}
