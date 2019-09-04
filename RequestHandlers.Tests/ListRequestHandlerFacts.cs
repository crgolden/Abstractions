namespace crgolden.Abstractions.RequestHandlers.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNet.OData.Query;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ListRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(ListRequestHandlerFacts).FullName;

        [Fact]
        public async Task List()
        {
            // Arrange
            const int count = 3;
            var entities = new FakeEntity[count];
            var models = new FakeEntity[count];
            var databaseName = $"{DatabaseNamePrefix}.{nameof(List)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            for (var i = 0; i < count; i++)
            {
                entities[i] = new FakeEntity($"Entity {i + 1}");
                models[i] = new FakeEntity($"Model {i + 1}");
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
                    It.IsAny<Expression<Func<FakeEntity, object>>[]>()))
                .Returns(models.AsQueryable());
            var request = new Mock<ListRequest<FakeEntity, FakeEntity>>(GetQueryOptions());
            IQueryable<FakeEntity> list;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeListRequestHandler(context, mapper.Object);
                list = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Collection(list, new Action<FakeEntity>[]
            {
                model => { Assert.Equal(models[0].Name, model.Name); },
                model => { Assert.Equal(models[1].Name, model.Name); },
                model => { Assert.Equal(models[2].Name, model.Name); }
            });
        }

        private static ODataQueryOptions<FakeEntity> GetQueryOptions()
        {
            var collection = new ServiceCollection();
            collection.AddOData();
            collection.AddODataQueryFilter();
            var provider = collection.BuildServiceProvider();
            var applicationBuilder = Mock.Of<IApplicationBuilder>(x => x.ApplicationServices == provider);
            var routeBuilder = new RouteBuilder(applicationBuilder);
            routeBuilder.EnableDependencyInjection();
            var modelBuilder = new ODataConventionModelBuilder(provider);
            var entitySet = modelBuilder.EntitySet<FakeEntity>("FakeEntities");
            entitySet.EntityType.HasKey(entity => entity.Id);
            var httpContext = new DefaultHttpContext { RequestServices = provider };
            return new ODataQueryOptions<FakeEntity>(
                context: new ODataQueryContext(
                    model: modelBuilder.GetEdmModel(),
                    elementClrType: typeof(FakeEntity),
                    path: new Microsoft.AspNet.OData.Routing.ODataPath()),
                request: new DefaultHttpRequest(httpContext));
        }
    }
}
