namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class DeleteRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DeleteRequestHandlerFacts).FullName;

        [Fact]
        public async Task DeleteRange()
        {
            // Arrange
            var entities = new []
            {
                new FakeEntity("Name 1"),
                new FakeEntity("Name 2"),
                new FakeEntity("Name 3")
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(DeleteRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }
            var keyValues = new[]
            {
                new object[] { entities[0].Id },
                new object[] { entities[1].Id },
                new object[] { entities[2].Id }
            };
            var request = new Mock<DeleteRangeRequest>(new object[] { keyValues });

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeDeleteRangeRequestHandler(context);
                await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    Assert.Null(await context.FindAsync<FakeEntity>(keyValues[i]));
                }
            }
        }
    }
}
