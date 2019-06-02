namespace crgolden.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public abstract class Entity
    {
        public DateTime Created { get; private set; }

        public DateTime? Updated { get; set; }

        protected Entity()
        {
            Created = DateTime.UtcNow;
        }
    }
}
