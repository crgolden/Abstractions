namespace Clarity.Abstractions.Controllers.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Moq;
    using Shared;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ClassControllerFacts
    {
        private readonly Mock<IMediator> _mediator;

        public ClassControllerFacts()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task List_Ok()
        {
            // Arrange
            var request = new DataSourceRequest();
            var dataSourceResult = new DataSourceResult();
            _mediator.Setup(x => x.Send(
                    It.Is<ListRequest<object, object>>(y => y.Request == request),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(dataSourceResult);
            var cache = new Mock<IMemoryCache>();
            cache.Setup(x => x.CreateEntry(It.IsAny<DataSourceRequest>())).Returns(Mock.Of<ICacheEntry>());
            var controller = new FakeClassController(_mediator.Object, cache.Object, Mock.Of<IOptions<CacheOptions>>());

            // Act
            var list = await controller.List(request);

            // Assert
            var result = Assert.IsType<OkObjectResult>(list);
            _mediator.Verify(x => x.Publish(
                It.IsAny<ListNotification>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(dataSourceResult, result.Value);
        }

        [Fact]
        public async Task Read_Ok()
        {
            // Arrange
            var model = new { Name = "Name", Id = Guid.NewGuid() };
            var keyValues = new object[] { model.Id };
            _mediator.Setup(x => x.Send(
                    It.Is<ReadRequest<object, object>>(y => y.KeyValues[0] == keyValues[0]),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);
            var cache = new Mock<IMemoryCache>();
            cache.Setup(x => x.CreateEntry(It.IsAny<object[]>())).Returns(Mock.Of<ICacheEntry>());
            var controller = new FakeClassController(_mediator.Object, cache.Object, Mock.Of<IOptions<CacheOptions>>());

            // Act
            var read = await controller.Read(keyValues);

            // Assert
            var result = Assert.IsType<OkObjectResult>(read);
            _mediator.Verify(x => x.Publish(
                It.IsAny<ReadNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Read_Bad_Request_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.Is<ReadRequest<object, object>>(y => y.KeyValues[0] == keyValues[0]),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeClassController(_mediator.Object, Mock.Of<IMemoryCache>(), Mock.Of<IOptions<CacheOptions>>());

            // Act
            var read = await controller.Read(keyValues);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(read);
            _mediator.Verify(x => x.Publish(
                It.IsAny<ReadNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(keyValues, result.Value);
        }

        [Fact]
        public async Task Read_Not_Found_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.Is<ReadRequest<object, object>>(y => y.KeyValues[0] == keyValues[0]),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(object));
            var controller = new FakeClassController(_mediator.Object, Mock.Of<IMemoryCache>(), Mock.Of<IOptions<CacheOptions>>());

            // Act
            var read = await controller.Read(keyValues);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(read);
            _mediator.Verify(x => x.Publish(
                It.IsAny<ReadNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(keyValues, result.Value);
        }

        [Fact]
        public async Task Update_No_Content()
        {
            // Arrange
            var model = new { Name = "Name" };
            var cache = new Mock<IMemoryCache>();
            cache.Setup(x => x.CreateEntry(It.IsAny<object[]>())).Returns(Mock.Of<ICacheEntry>());
            var controller = new FakeClassController(_mediator.Object, cache.Object, Mock.Of<IOptions<CacheOptions>>());

            // Act
            var update = await controller.Update(model);

            // Assert
            Assert.IsType<NoContentResult>(update);
            _mediator.Verify(x => x.Publish(
                It.IsAny<UpdateNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_Bad_Request_Object()
        {
            // Arrange
            var model = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<UpdateRequest<object, object>>(y => y.Model == model),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeClassController(_mediator.Object, Mock.Of<IMemoryCache>(), Mock.Of<IOptions<CacheOptions>>());

            // Act
            var update = await controller.Update(model);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(update);
            _mediator.Verify(x => x.Publish(
                It.IsAny<UpdateNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Create_Ok()
        {
            // Arrange
            var model = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRequest<object, object>>(y => y.Model == model),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((model, new object[]{ new {} }));
            var cache = new Mock<IMemoryCache>();
            cache.Setup(x => x.CreateEntry(It.IsAny<object[]>())).Returns(Mock.Of<ICacheEntry>());
            var controller = new FakeClassController(_mediator.Object, cache.Object, Mock.Of<IOptions<CacheOptions>>());

            // Act
            var create = await controller.Create(model);

            // Assert
            var result = Assert.IsType<OkObjectResult>(create);
            _mediator.Verify(x => x.Publish(
                It.IsAny<CreateNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Create_Bad_Request_Object()
        {
            // Arrange
            var model = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRequest<object, object>>(y => y.Model == model),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeClassController(_mediator.Object, Mock.Of<IMemoryCache>(), Mock.Of<IOptions<CacheOptions>>());

            // Act
            var create = await controller.Create(model);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(create);
            _mediator.Verify(x => x.Publish(
                It.IsAny<CreateNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Delete_No_Content()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            var controller = new FakeClassController(_mediator.Object, Mock.Of<IMemoryCache>(), Mock.Of<IOptions<CacheOptions>>());

            // Act
            var delete = await controller.Delete(keyValues);

            // Assert
            Assert.IsType<NoContentResult>(delete);
            _mediator.Verify(x => x.Publish(
                It.IsAny<DeleteNotification>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Delete_Bad_Request_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.Is<DeleteRequest>(y => y.KeyValues[0] == keyValues[0]),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeClassController(_mediator.Object, Mock.Of<IMemoryCache>(), Mock.Of<IOptions<CacheOptions>>());

            // Act
            var delete = await controller.Delete(keyValues);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            _mediator.Verify(x => x.Publish(
                It.IsAny<DeleteNotification>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(keyValues, result.Value);
        }
    }
}
