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

    public class EditRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(EditRangeRequestHandlerFacts).FullName;

        [Fact]
        public async Task EditRange()
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
                Mock.Of<object>(),
                Mock.Of<object>(),
                Mock.Of<object>()
            }.AsEnumerable();
            var databaseName = $"{DatabaseNamePrefix}.{nameof(EditRange)}";
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

            var request = new Mock<EditRangeRequest<FakeEntity, object>>(models);
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<IEnumerable<FakeEntity>>(It.IsAny<IEnumerable<object>>())).Returns(entities);

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeEditRangeRequestHandler(context, mapper.Object);
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