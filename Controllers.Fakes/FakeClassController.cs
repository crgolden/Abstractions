namespace Clarity.Abstractions.Fakes
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Controllers;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Moq;
    using Shared;

    [ExcludeFromCodeCoverage]
    internal class FakeClassController : ClassController<object, object, object>
    {
        public FakeClassController(IMediator mediator, IMemoryCache cache, IOptions<CacheOptions> cacheOptions)
            : base(mediator, cache, cacheOptions)
        {
        }

        public override Task<IActionResult> List(DataSourceRequest request)
        {
            return List(
                request: new Mock<ListRequest<object, object>>(new ModelStateDictionary(), request).Object,
                notification: Mock.Of<ListNotification>());
        }

        public override Task<IActionResult> Read(object[] keyValues)
        {
            return Read(
                request: new Mock<ReadRequest<object, object>>(new object[] { keyValues }).Object,
                notification: Mock.Of<ReadNotification<object>>());
        }

        public override Task<IActionResult> Update(object model)
        {
            return Update(
                request: new Mock<UpdateRequest<object, object>>(model).Object,
                notification: Mock.Of<UpdateNotification<object>>());
        }

        public override Task<IActionResult> Create(object model)
        {
            return Create(
                request: new Mock<CreateRequest<object, object>>(model).Object,
                notification: Mock.Of<CreateNotification<object>>());
        }

        public override Task<IActionResult> Delete(object[] keyValues)
        {
            return Delete(
                request: new Mock<DeleteRequest>(new object[] { keyValues }).Object,
                notification: Mock.Of<DeleteNotification>());
        }
    }
}
