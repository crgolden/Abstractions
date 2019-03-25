namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;
    using Xunit;

    public class DeleteRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DeleteRequestHandlerFacts).FullName;

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var entity = new FakeEntity("Name");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Delete)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Add(entity);
                await context.SaveChangesAsync();
            }

            var keyValues = new object[] { entity.Id };
            var cache = new Mock<IMemoryCache>();
            cache.Setup(x => x.CreateEntry(It.IsAny<object[]>())).Returns(Mock.Of<ICacheEntry>());
            var request = new Mock<DeleteRequest>(new object[] { keyValues });

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeDeleteRequestHandler(context, cache.Object);
                await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                entity = await context.FindAsync<FakeEntity>(keyValues);
                Assert.Null(entity);
            }
        }
    }
}