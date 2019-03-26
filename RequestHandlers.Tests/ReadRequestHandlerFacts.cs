namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ReadRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(ReadRequestHandlerFacts).FullName;

        [Fact]
        public async Task Read()
        {
            // Arrange
            var entity = new FakeEntity("Name");
            var model = new object();
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Read)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Add(entity);
                await context.SaveChangesAsync();
            }

            var keyValues = new object[] { entity.Id };
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<object>(It.Is<FakeEntity>(y => y.Name == entity.Name))).Returns(model);
            var request = new Mock<ReadRequest<FakeEntity, object>>(new object[] { keyValues });
            object read;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeReadRequestHandler(context, mapper.Object);
                read = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Equal(model, read);
        }
    }
}