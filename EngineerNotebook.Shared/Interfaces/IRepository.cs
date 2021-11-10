using Ardalis.Specification;

namespace EngineerNotebook.Shared.Interfaces;
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot { }
