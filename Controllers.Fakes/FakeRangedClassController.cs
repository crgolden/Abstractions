namespace crgolden.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Controllers;
    using MediatR;
    using Microsoft.AspNet.OData.Query;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Moq;
    using Shared;

    [ExcludeFromCodeCoverage]
    internal class FakeRangedClassController : RangedClassController<object, object, object>
    {
        public FakeRangedClassController(IMediator mediator, IMemoryCache cache, IOptions<CacheOptions> cacheOptions)
            : base(mediator, cache, cacheOptions)
        {
        }

        public override Task<IActionResult> List(ODataQueryOptions<object> options)
        {
            return Task.FromResult(Mock.Of<IActionResult>());
        }

        public override Task<IActionResult> Read(object[] keyValues)
        {
            return Task.FromResult(Mock.Of<IActionResult>());
        }

        public override Task<IActionResult> ReadRange(object[][] keyValues)
        {
            return ReadRange(
                request: new Mock<ReadRangeRequest<object, object>>(new object[] { keyValues }).Object,
                notification: Mock.Of<ReadRangeNotification<object>>());
        }

        public override Task<IActionResult> Update(object model)
        {
            return Task.FromResult(Mock.Of<IActionResult>());
        }

        public override Task<IActionResult> UpdateRange(object[] models)
        {
            return UpdateRange(
                request: new Mock<UpdateRangeRequest<object, object>>(new object[] { models }).Object,
                notification: Mock.Of<UpdateRangeNotification<object>>());
        }

        public override Task<IActionResult> Create(object model)
        {
            return Task.FromResult(Mock.Of<IActionResult>());
        }

        public override Task<IActionResult> CreateRange(object[] models)
        {
            return CreateRange(
                request: new Mock<CreateRangeRequest<object, object>>(new object[] { models }).Object,
                notification: Mock.Of<CreateRangeNotification<object>>());
        }

        public override Task<IActionResult> Delete(object[] keyValues)
        {
            return Task.FromResult(Mock.Of<IActionResult>());
        }

        public override Task<IActionResult> DeleteRange(object[][] keyValues)
        {
            return DeleteRange(
                request: new Mock<DeleteRangeRequest>(new object[] { keyValues }).Object,
                notification: Mock.Of<DeleteRangeNotification>());
        }
    }
}
