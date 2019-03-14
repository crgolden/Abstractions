namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CreateRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRangeRequestHandlerFacts).FullName;

        [Fact]
        public async Task CreateRange()
        {
            // Arrange
            var entity1 = new FakeEntity("Name 1");
            var entity2 = new FakeEntity("Name 2");
            var entity3 = new FakeEntity("Name 3");
            var entities = new []
            {
                entity1,
                entity2,
                entity3
            };
            var models = new []
            {
                Mock.Of<object>(),
                Mock.Of<object>(),
                Mock.Of<object>()
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(CreateRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var request = new Mock<CreateRangeRequest<IEnumerable<object>, FakeEntity, object>>(models.AsEnumerable());
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<IEnumerable<FakeEntity>>(It.IsAny<IEnumerable<object>>())).Returns(entities);
            mapper.Setup(x => x.Map<IEnumerable<object>>(It.IsAny<IEnumerable<FakeEntity>>())).Returns(models.AsEnumerable());
            IEnumerable<object> createRange;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeCreateRangeRequestHandler(context, mapper.Object);
                createRange = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Equal(models.Length, createRange.Count());
            using (var context = new FakeContext(options))
            {
                foreach (var entity in entities)
                {
                    Assert.NotNull(await context.Set<FakeEntity>().SingleOrDefaultAsync(x => x.Name.Equals(entity.Name)));
                }
            }
        }
    }
}
