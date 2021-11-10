namespace EngineerNotebook.Shared.Endpoints;
/// <summary>
/// Way of identifying client-facing data model
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IDto {}

/// <summary>
/// Way of identifying client-facing response of a data-model
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IDtoResponse<TEntity>
    where TEntity : class, new()
{
    TEntity Result { get; set; }
}

public interface IDtoResponseCollection<TEntity>
    where TEntity : class, new()
{
    List<TEntity> Results { get; set; }
}
