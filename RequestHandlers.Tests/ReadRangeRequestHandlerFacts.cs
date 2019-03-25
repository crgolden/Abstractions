namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;
    using Xunit;

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

            var mapper = new Mock<IMapper>();
            for (var i = 0; i < count; i++)
            {
                var entity = entities[i];
                models[i] = new object();
                keyValues[i] = new object[] { entity.Id };
                mapper.Setup(x => x.Map<object>(It.Is<FakeEntity>(y => y.Id == entity.Id))).Returns(models[i]);
            }

            var cache = new Mock<IMemoryCache>();
            cache.Setup(x => x.CreateEntry(It.IsAny<object[]>())).Returns(Mock.Of<ICacheEntry>());
            var request = new Mock<ReadRangeRequest<FakeEntity, object>>(new object[] { keyValues });
            object[] readRange;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeReadRangeRequestHandler(context, mapper.Object, cache.Object);
                readRange = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Equal(models, readRange);
        }
    }
}