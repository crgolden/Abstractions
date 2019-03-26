namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class UpdateRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(UpdateRequestHandlerFacts).FullName;

        [Fact]
        public async Task Update()
        {
            // Arrange
            const string newName = "New Name";
            var entity = new FakeEntity("Name");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Update)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            using (var context = new FakeContext(options))
            {
                context.Add(entity);
                await context.SaveChangesAsync();
            }

            entity.Name = newName;
            var keyValues = new object[] { entity.Id };
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<FakeEntity>(It.IsAny<object>())).Returns(entity);
            var request = new Mock<UpdateRequest<FakeEntity, object>>(new object());

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeUpdateRequestHandler(context, mapper.Object);
                await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                entity = await context.FindAsync<FakeEntity>(keyValues);
                Assert.Equal(newName, entity.Name);
            }
        }
    }
}
