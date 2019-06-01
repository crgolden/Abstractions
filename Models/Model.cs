namespace crgolden.Abstractions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public abstract class Model
    {
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
