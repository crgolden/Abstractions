namespace crgolden.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;

    [ExcludeFromCodeCoverage]
    public abstract class DeleteRangeRequest : IRequest<object[][]>
    {
        public readonly object[][] KeyValues;

        protected DeleteRangeRequest(object[][] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
