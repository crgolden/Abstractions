namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class EditRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(EditRequestHandlerFacts).FullName;

        [Fact]
        public async Task Edit()
        {
            // Arrange
            const string newName = "New Name";
            var entity = new FakeEntity("Name");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Edit)}";
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
            var request = new Mock<EditRequest<FakeEntity, object>>(Mock.Of<object>());
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<FakeEntity>(It.IsAny<object>())).Returns(entity);

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeEditRequestHandler(context, mapper.Object);
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
