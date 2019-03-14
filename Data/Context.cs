namespace Clarity.Abstractions
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public abstract class Context : DbContext
    {
        protected Context(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamp(DateTime.UtcNow);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken token = default)
        {
            AddTimestamp(DateTime.UtcNow);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, token);
        }

        protected virtual void AddTimestamp(DateTime timestamp)
        {
            foreach (var entity in ChangeTracker
                .Entries()
                .Where(x => x.Entity is Entity && x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .Cast<Entity>())
            {
                entity.Updated = timestamp;
            }
        }
    }
}
