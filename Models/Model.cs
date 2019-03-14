namespace Clarity.Abstractions
{
    using System;

    public abstract class Model
    {
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
