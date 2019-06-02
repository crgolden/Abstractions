namespace crgolden.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public abstract class Entity
    {
        public readonly DateTime Created;

        public DateTime? Updated { get; set; }

        protected Entity()
        {
            Created = DateTime.UtcNow;
        }
    }
}
