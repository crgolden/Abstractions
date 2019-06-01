namespace crgolden.Abstractions.Controllers.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Moq;
    using Shared;
    using Xunit;

    [ExcludeFromCodeCoverage]
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
            var keyValues = new[]
            {
                new object[] {new { }},
                new object[] {new { }},
                new object[] {new { }}
            };
            _mediator.Setup(x => x.Send(
                    It.Is<UpdateRangeRequest<object, object>>(y => y.Models == models),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(keyValues);
            var cache = new Mock<IMemoryCache>();
            cache.Setup(x => x.CreateEntry(It.IsAny<object[]>())).Returns(Mock.Of<ICacheEntry>());
            var controller = new FakeRangedClassController(_mediator.Object, cache.Object, Mock.Of<IOptions<CacheOptions>>());

            // Act
            var updateRange = await controller.UpdateRange(models);

            // Assert
            Assert.IsType<NoContentResult>(updateRange);
            _mediator.Verify(x => x.Publish(
                It.IsAny<UpdateRangeNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
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
                    It.Is<UpdateRangeRequest<object, object>>(y => y.Models == models),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeRangedClassController(_mediator.Object, Mock.Of<IMemoryCache>(), Mock.Of<IOptions<CacheOptions>>());

            // Act
            var updateRange = await controller.UpdateRange(models);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(updateRange);
            _mediator.Verify(x => x.Publish(
                It.IsAny<UpdateRangeNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
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
            var keyValues = new[]
            {
                new object[] {new { }},
                new object[] {new { }},
                new object[] {new { }}
            };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRangeRequest<object, object>>(y => y.Models == models),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((models, keyValues));
            var cache = new Mock<IMemoryCache>();
            cache.Setup(x => x.CreateEntry(It.IsAny<object[]>())).Returns(Mock.Of<ICacheEntry>());
            var controller = new FakeRangedClassController(_mediator.Object, cache.Object, Mock.Of<IOptions<CacheOptions>>());

            // Act
            var createRange = await controller.CreateRange(models);

            // Assert
            var result = Assert.IsType<OkObjectResult>(createRange);
            _mediator.Verify(x => x.Publish(
                It.IsAny<CreateRangeNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
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
                    It.Is<CreateRangeRequest<object, object>>(y => y.Models == models),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeRangedClassController(_mediator.Object, Mock.Of<IMemoryCache>(), Mock.Of<IOptions<CacheOptions>>());

            // Act
            var createRange = await controller.CreateRange(models);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(createRange);
            _mediator.Verify(x => x.Publish(
                It.IsAny<CreateRangeNotification<object>>(),
                It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.Equal(models, result.Value);
        }
    }
}
