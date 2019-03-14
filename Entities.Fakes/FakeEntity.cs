namespace Clarity.Abstractions.Fakes
{
    using System;

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