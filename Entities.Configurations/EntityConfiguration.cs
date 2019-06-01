namespace crgolden.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    [ExcludeFromCodeCoverage]
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> entity)
        {
            entity.Property(e => e.Created).HasDefaultValueSql("getutcdate()");
            entity.Property(e => e.Updated);
        }
    }
}
