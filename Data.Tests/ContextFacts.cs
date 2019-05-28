namespace Clarity.Abstractions.Data.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ContextFacts
    {
        private static string DatabaseNamePrefix => typeof(ContextFacts).FullName;

        [Fact]
        public void SaveChanges()
        {
            // Arrange
            var entity = new FakeEntity("Name");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(SaveChanges)}";
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Add(entity);
                context.SaveChanges(true);
            }

            // Act
            using (var context = new FakeContext(options))
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges(true);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                entity = context.Find<FakeEntity>(entity.Id);
            }

            Assert.NotNull(entity?.Updated);
        }

        [Fact]
        public async Task SaveChangesAsync()
        {
            // Arrange
            var entity = new FakeEntity("Name");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(SaveChangesAsync)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Add(entity);
                await context.SaveChangesAsync(true);
            }

            // Act
            using (var context = new FakeContext(options))
            {
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync(true);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                entity = await context.FindAsync<FakeEntity>(entity.Id);
            }

            Assert.NotNull(entity?.Updated);
        }
    }
}