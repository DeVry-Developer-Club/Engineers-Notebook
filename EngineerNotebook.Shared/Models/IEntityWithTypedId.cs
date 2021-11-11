namespace EngineerNotebook.Shared.Models;
public interface IEntityWithTypedId<TId>
{
    TId Id { get; set; }
}
