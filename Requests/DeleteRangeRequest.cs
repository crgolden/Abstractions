namespace Clarity.Abstractions
{
    using MediatR;

    public abstract class DeleteRangeRequest : IRequest<object[][]>
    {
        public readonly object[][] KeyValues;

        protected DeleteRangeRequest(object[][] keyValues)
        {
            KeyValues = keyValues;
        }
    }
}
