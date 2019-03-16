namespace Clarity.Abstractions.Controllers.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

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
                    It.Is<ListRequest<object, object>>(y => y.Request.Equals(request)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(dataSourceResult);
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var list = await controller.List(request);

            // Assert
            var result = Assert.IsType<OkObjectResult>(list);
            Assert.Equal(dataSourceResult, result.Value);
        }

        [Fact]
        public async Task Read_Ok()
        {
            // Arrange
            var model = new { Name = "Name", Id = Guid.NewGuid() };
            var keyValues = new object[] { model.Id };
            _mediator.Setup(x => x.Send(
                    It.Is<ReadRequest<object, object>>(y => y.KeyValues[0].Equals(keyValues[0])),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var read = await controller.Read(keyValues);

            // Assert
            var result = Assert.IsType<OkObjectResult>(read);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Read_Bad_Request_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.Is<ReadRequest<object, object>>(y => y.KeyValues[0].Equals(keyValues[0])),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var read = await controller.Read(keyValues);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(read);
            Assert.Equal(keyValues, result.Value);
        }

        [Fact]
        public async Task Read_Not_Found_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.Is<ReadRequest<object, object>>(y => y.KeyValues[0].Equals(keyValues[0])),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(object));
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var read = await controller.Read(keyValues);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(read);
            Assert.Equal(keyValues, result.Value);
        }

        [Fact]
        public async Task Update_No_Content()
        {
            // Arrange
            var model = new { Name = "Name" };
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var update = await controller.Update(model);

            // Assert
            Assert.IsType<NoContentResult>(update);
        }

        [Fact]
        public async Task Update_Bad_Request_Object()
        {
            // Arrange
            var model = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<UpdateRequest<object, object>>(y => y.Model.Equals(model)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var update = await controller.Update(model);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(update);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Create_Ok()
        {
            // Arrange
            var model = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRequest<object, object>>(y => y.Model.Equals(model)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var create = await controller.Create(model);

            // Assert
            var result = Assert.IsType<OkObjectResult>(create);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Create_Bad_Request_Object()
        {
            // Arrange
            var model = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRequest<object, object>>(y => y.Model.Equals(model)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var create = await controller.Create(model);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(create);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Delete_No_Content()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var delete = await controller.Delete(keyValues);

            // Assert
            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_Bad_Request_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.Is<DeleteRequest>(y => y.KeyValues[0].Equals(keyValues[0])),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeClassController(_mediator.Object);

            // Act
            var delete = await controller.Delete(keyValues);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.Equal(keyValues, result.Value);
        }
    }
}
