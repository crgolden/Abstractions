namespace Clarity.Abstractions.Fakes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Controllers;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Moq;

    internal class FakeController : Controller<object, object, object>
    {
        public FakeController(IMediator mediator) : base(mediator)
        {
        }

        public override Task<IActionResult> Index(DataSourceRequest request)
        {
            return Index(
                request: new Mock<IndexRequest<object, object>>(new ModelStateDictionary(), request).Object,
                notification: Mock.Of<IndexNotification>());
        }

        public override Task<IActionResult> Details(object[] keyValues)
        {
            return Details(
                request: new Mock<DetailsRequest<object, object>>(new object[] { keyValues }).Object,
                notification: Mock.Of<DetailsNotification<object>>());
        }

        public override Task<IActionResult> Edit(object model)
        {
            return Edit(
                request: new Mock<EditRequest<object, object>>(model).Object,
                notification: Mock.Of<EditNotification<object>>());
        }

        public override Task<IActionResult> EditRange(IEnumerable<object> models)
        {
            return EditRange(
                request: new Mock<EditRangeRequest<object, object>>(models).Object,
                notification: Mock.Of<EditRangeNotification<object>>());
        }

        public override Task<IActionResult> Create(object model)
        {
            return Create(
                request: new Mock<CreateRequest<object, object>>(model).Object,
                notification: Mock.Of<CreateNotification<object>>());
        }

        public override Task<IActionResult> CreateRange(IEnumerable<object> models)
        {
            return CreateRange(
                request: new Mock<CreateRangeRequest<IEnumerable<object>, object, object>>(models).Object,
                notification: Mock.Of<CreateRangeNotification<object>>());
        }

        public override Task<IActionResult> Delete(object[] keyValues)
        {
            return Delete(
                request: new Mock<DeleteRequest>(new object[] { keyValues }).Object,
                notification: Mock.Of<DeleteNotification>());
        }
    }
}
