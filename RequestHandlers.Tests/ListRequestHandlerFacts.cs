namespace Clarity.Abstractions.RequestHandlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Kendo.Mvc.UI;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ListRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(ListRequestHandlerFacts).FullName;

        [Fact]
        public async Task List()
        {
            // Arrange
            const int count = 3;
            var entities = new FakeEntity[count];
            var models = new object[count];
            var databaseName = $"{DatabaseNamePrefix}.{nameof(List)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            for (var i = 0; i < count; i++)
            {
                entities[i] = new FakeEntity($"Name {i + 1}");
                models[i] = new object();
            }

            using (var context = new FakeContext(options))
            {
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.ProjectTo(
                    It.IsAny<IQueryable>(),
                    It.IsAny<object>(),
                    It.IsAny<Expression<Func<object, object>>[] > ()))
                .Returns(models.AsQueryable());
            var request = new Mock<ListRequest<FakeEntity, object>>(null, new DataSourceRequest());
            DataSourceResult list;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeListRequestHandler(context, mapper.Object);
                list = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            var result = Assert.IsType<DataSourceResult>(list);
            var data = Assert.IsAssignableFrom<IEnumerable<object>>(result.Data);
            Assert.Equal(count, data.Count());
        }
    }
}
