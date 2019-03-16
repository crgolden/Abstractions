namespace Clarity.Abstractions.Controllers.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class RangedClassControllerFacts
    {
        private readonly Mock<IMediator> _mediator;

        public RangedClassControllerFacts()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task UpdateRange_No_Content()
        {
            // Arrange
            var models = new object[]
            {
                new { Name = "Name 1" },
                new { Name = "Name 2" },
                new { Name = "Name 3" }
            };
            var controller = new FakeRangedClassController(_mediator.Object);

            // Act
            var updateRange = await controller.UpdateRange(models);

            // Assert
            Assert.IsType<NoContentResult>(updateRange);
        }

        [Fact]
        public async Task UpdateRange_Bad_Request_Object()
        {
            // Arrange
            var models = new object[]
            {
                new { Name = "Name 1" },
                new { Name = "Name 2" },
                new { Name = "Name 3" }
            };
            _mediator.Setup(x => x.Send(
                    It.Is<UpdateRangeRequest<object, object>>(y => y.Models.Equals(models)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeRangedClassController(_mediator.Object);

            // Act
            var updateRange = await controller.UpdateRange(models);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(updateRange);
            Assert.Equal(models, result.Value);
        }

        [Fact]
        public async Task CreateRange_Ok()
        {
            // Arrange
            var models = new object[]
            {
                new { Name = "Name 1" },
                new { Name = "Name 2" },
                new { Name = "Name 3" }
            };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRangeRequest<object, object>>(y => y.Models.Equals(models)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(models);
            var controller = new FakeRangedClassController(_mediator.Object);

            // Act
            var createRange = await controller.CreateRange(models);

            // Assert
            var result = Assert.IsType<OkObjectResult>(createRange);
            Assert.Equal(models, result.Value);
        }

        [Fact]
        public async Task CreateRange_Bad_Request_Object()
        {
            // Arrange
            var models = new object[]
            {
                new { Name = "Name 1" },
                new { Name = "Name 2" },
                new { Name = "Name 3" }
            };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRangeRequest<object, object>>(y => y.Models.Equals(models)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeRangedClassController(_mediator.Object);

            // Act
            var createRange = await controller.CreateRange(models);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(createRange);
            Assert.Equal(models, result.Value);
        }
    }
}
