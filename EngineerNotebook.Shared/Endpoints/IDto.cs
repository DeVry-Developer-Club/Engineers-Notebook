namespace EngineerNotebook.Shared.Endpoints;

public interface IDto
{
    
}

public interface IDtoResponse<TEntity> where TEntity : class, new()
{
    TEntity Result { get; set; }
}

public interface IDtoResponseCollection<TEntity> where TEntity : class, new()
{
    List<TEntity> Results { get; set; }
}