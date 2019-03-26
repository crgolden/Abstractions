namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class DeleteRequest : IRequest<object[][]>
    {
        public readonly object[] KeyValues;

        protected DeleteRequest(object[] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
