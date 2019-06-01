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
            var request = new Mock<DeleteRequest>(new object[] { keyValues });

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeDeleteRequestHandler(context);
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