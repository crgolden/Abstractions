namespace crgolden.Abstractions.RequestHandlers.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeleteRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DeleteRequestHandlerFacts).FullName;

        [Fact]
        public async Task DeleteRange()
        {
            // Arrange
            const int count = 3;
            var entities = new FakeEntity[count];
            var keyValues = new object[count][];
            var databaseName = $"{DatabaseNamePrefix}.{nameof(DeleteRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                for (var i = 0; i < count; i++) entities[i] = new FakeEntity($"Name {i + 1}");
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            for (var i = 0; i < count; i++) keyValues[i] = new object[] { entities[i].Id };
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
                for (var i = 0; i < count; i++)
                {
                    Assert.Null(await context.FindAsync<FakeEntity>(keyValues[i]));
                }
            }
        }
    }
}
