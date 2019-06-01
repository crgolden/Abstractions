namespace crgolden.Abstractions.RequestHandlers.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ReadRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(ReadRangeRequestHandlerFacts).FullName;

        [Fact]
        public async Task ReadRange()
        {
            // Arrange
            const int count = 3;
            var entities = new FakeEntity[count];
            var models = new object[count];
            var keyValues = new object[count][];
            var databaseName = $"{DatabaseNamePrefix}.{nameof(ReadRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                for (var i = 0; i < count; i++) entities[i] = new FakeEntity($"Name {i + 1}");
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            for (var i = 0; i < count; i++)
            {
                models[i] = new object();
                keyValues[i] = new object[] { entities[i].Id };
            }

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<object[]>(It.IsAny<FakeEntity[]>())).Returns(models);
            var request = new Mock<ReadRangeRequest<FakeEntity, object>>(new object[] { keyValues });
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