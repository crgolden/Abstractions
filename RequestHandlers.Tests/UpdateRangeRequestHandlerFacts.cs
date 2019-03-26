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
            const int count = 3;
            var entities = new FakeEntity[count];
            var models = new object[count];
            var databaseName = $"{DatabaseNamePrefix}.{nameof(UpdateRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                for (var i = 0; i < count; i++) entities[i] = new FakeEntity($"Name {i + 1}");
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            var mapper = new Mock<IMapper>();
            for (var i = 0; i < count; i++)
            {
                var model = new object();
                models[i] = model;
                entities[i].Name = $"New Name {i + 1}";
                mapper.Setup(x => x.Map<FakeEntity>(model)).Returns(entities[i]);
            }

            var request = new Mock<UpdateRangeRequest<FakeEntity, object>>(new object[] { models });

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeUpdateRangeRequestHandler(context, mapper.Object);
                await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                for (var i = 0; i < count; i++)
                {
                    var name = $"New Name {i + 1}";
                    Assert.NotNull(await context.Set<FakeEntity>().SingleOrDefaultAsync(x => x.Name == name));
                }
            }
        }
    }
}