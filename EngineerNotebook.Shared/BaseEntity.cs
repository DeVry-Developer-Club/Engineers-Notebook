using System.Collections.Generic;

namespace EngineerNotebook.Shared
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public List<BaseDomainEvent> Events = new();
    }
}