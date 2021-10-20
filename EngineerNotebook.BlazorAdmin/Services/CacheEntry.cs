using System;

namespace EngineerNotebook.BlazorAdmin.Services
{
    public class CacheEntry<T>
    {
        public T Value { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public CacheEntry(T item)
        {
            Value = item;
        }

        public CacheEntry (){}
        
    }
}