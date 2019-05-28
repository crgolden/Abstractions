namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CreateRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRangeRequestHandlerFacts).FullName;

        [Fact]
        public async Task CreateRange()
        {
            // Arrange
            const int count = 3;
            var entities = new FakeEntity[count];
            var models = new object[count];
            var databaseName = $"{DatabaseNamePrefix}.{nameof(CreateRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var mapper = new Mock<IMapper>();
            for (var i = 0; i < count; i++)
            {
                var entity = new FakeEntity($"Name {i + 1}");
                var model = new object();
                entities[i] = entity;
                models[i] = model;
                mapper.Setup(x => x.Map<FakeEntity>(model)).Returns(entity);
                mapper.Setup(x => x.Map<object>(It.Is<FakeEntity>(y => y.Name == entity.Name))).Returns(model);
            }

            var request = new Mock<CreateRangeRequest<FakeEntity, object>>(new object[] { models });
            (object[], object[][]) createRange;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeCreateRangeRequestHandler(context, mapper.Object);
                createRange = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Equal(models.Length, createRange.Item1.Length);
            using (var context = new FakeContext(options))
            {
                foreach (var entity in entities)
                {
                    Assert.NotNull(await context.Set<FakeEntity>().SingleOrDefaultAsync(x => x.Name == entity.Name));
                }
            }
        }
    }
}
