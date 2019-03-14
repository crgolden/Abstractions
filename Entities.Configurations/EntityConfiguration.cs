namespace Clarity.Abstractions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
