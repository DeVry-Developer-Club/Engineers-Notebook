using Ardalis.Specification;

namespace EngineerNotebook.Shared.Interfaces;
public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot { }