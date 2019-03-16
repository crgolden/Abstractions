namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ReadRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(ReadRangeRequestHandlerFacts).FullName;

        [Fact]
        public async Task ReadRange()
        {
            // Arrange
            var entities = new []
            {
                new FakeEntity("Name 1"),
                new FakeEntity("Name 2"),
                new FakeEntity("Name 3")
            };
            var models = new []
            {
                new object(),
                new object(),
                new object()
            };
            var keyValues = new []
            {
                new object[] { entities[0].Id },
                new object[] { entities[1].Id },
                new object[] { entities[2].Id }
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(ReadRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            var request = new Mock<ReadRangeRequest<FakeEntity, object>>(new object[] { keyValues });
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<object[]>(It.IsAny<FakeEntity[]>())).Returns(models);
            object[] readRange;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeReadRangeRequestHandler(context, mapper.Object);
                readRange = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Equal(models, readRange);
        }
    }
}