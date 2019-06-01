namespace crgolden.Abstractions.Fakes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal class FakeEntity : Entity
    {
        internal Guid Id { get; private set; }

        internal string Name { get; set; }

        internal FakeEntity(string name)
        {
            Name = name;
        }
    }
}
