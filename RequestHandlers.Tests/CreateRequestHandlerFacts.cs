namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CreateRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRequestHandlerFacts).FullName;

        [Fact]
        public async Task Create()
        {
            // Arrange
            var entity = new FakeEntity("Name");
            var model = Mock.Of<object>();
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Create)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var request = new Mock<CreateRequest<FakeEntity, object>>(model);
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<FakeEntity>(model)).Returns(entity);
            mapper.Setup(x => x.Map<object>(It.Is<FakeEntity>(y => y.Name == entity.Name))).Returns(model);
            object create;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeCreateRequestHandler(context, mapper.Object);
                create = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Equal(model, create);
            using (var context = new FakeContext(options))
            {
                Assert.NotNull(await context.Set<FakeEntity>().SingleOrDefaultAsync(x => x.Name.Equals(entity.Name)));
            }
        }
    }
}
