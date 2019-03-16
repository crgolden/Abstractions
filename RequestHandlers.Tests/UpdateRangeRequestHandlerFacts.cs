namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class UpdateRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(UpdateRangeRequestHandlerFacts).FullName;

        [Fact]
        public async Task UpdateRange()
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
            var databaseName = $"{DatabaseNamePrefix}.{nameof(UpdateRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            using (var context = new FakeContext(options))
            {
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].Name = $"New Name {i}";
            }

            var request = new Mock<UpdateRangeRequest<FakeEntity, object>>(new object[] { models });
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<FakeEntity[]>(It.IsAny<object[]>())).Returns(entities);

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeUpdateRangeRequestHandler(context, mapper.Object);
                await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    var name = $"New Name {i}";
                    Assert.NotNull(await context.Set<FakeEntity>().SingleOrDefaultAsync(x => x.Name.Equals(name)));
                }
            }
        }
    }
}