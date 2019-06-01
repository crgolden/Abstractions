namespace crgolden.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using MediatR;

    [ExcludeFromCodeCoverage]
    public abstract class DeleteRequest : IRequest<object[][]>
    {
        public readonly object[] KeyValues;

        protected DeleteRequest(object[] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
