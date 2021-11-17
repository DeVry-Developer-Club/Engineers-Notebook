using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Models;
namespace EngineerNotebook.Infrastructure.Data;

public class BaseDbService<TEntity> : IAsyncRepository<TEntity> where TEntity : class, IEntityWithTypedId<string>
{
    protected readonly IMongoCollection<TEntity> Collection;

    public BaseDbService(IDatabaseOptions options)
    {
        var client = new MongoClient(options.FullConnectionString);
        var database = client.GetDatabase(options.DatabaseName);
        Collection = database.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        var count = await Collection.EstimatedDocumentCountAsync(cancellationToken: cancellationToken);

        return count > 0;
    }

    public virtual async Task<IList<TEntity>> Find(FilterDefinition<TEntity> filter, CancellationToken cancellationToken = default)
    {
        return await Collection.Aggregate()
            .Match(filter)
            .ToListAsync();
    }

    public virtual async Task<IList<TEntity>> Get(CancellationToken cancellationToken = default)
    {
        return await Collection.GetAllAsync(cancellationToken);
    }

    public virtual async Task<ResultOf<TEntity>> Find(string id, CancellationToken cancellationToken = default)
    {
        var value = (await Collection.FindAsync(x => x.Id == id, cancellationToken: cancellationToken)).FirstOrDefault();

        if (value is null)
            return ResultOf<TEntity>.Failure($"Could not locate entry with Id: {id}", (int)HttpStatusCode.NotFound);

        return new()
        {
            Value = value
        };
    }

    public virtual async Task<IList<TEntity>> Find(Predicate<TEntity> condition, CancellationToken cancellationToken = default)
    {
        var result = await Collection.FindAsync(x => condition(x), cancellationToken: cancellationToken);
        return await result.ToListAsync();
    }

    public virtual async Task<ResultOf<TEntity>> Create(TEntity data, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(data, cancellationToken: cancellationToken);

        return new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Value = data
        };
    }

    public virtual async Task<ResultOf<TEntity>> Update(TEntity model, CancellationToken cancellationToken = default)
    {
        var result = await Collection.ReplaceOneAsync(x => x.Id == model.Id, model, cancellationToken: cancellationToken);


        if (result.IsAcknowledged && result.ModifiedCount == 0)
            return ResultOf<TEntity>.Failure($"Could not locate item with id: {model.Id}");

        return new ResultOf<TEntity>()
        {
            StatusCode = (int)HttpStatusCode.NoContent
        };
    }

    public virtual async Task<ResultOf<TEntity>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(x=>x.Id == id, cancellationToken: cancellationToken);

        if (result.IsAcknowledged && result.DeletedCount == 0)
            return ResultOf<TEntity>.Failure($"Could not locate item with id: {id}");

        return new()
        {
            StatusCode = (int)HttpStatusCode.NoContent
        };
    }

    public virtual async Task<ResultOf<TEntity>> Delete(Predicate<TEntity> condition, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(x => condition(x), cancellationToken: cancellationToken);

        if (result.IsAcknowledged && result.DeletedCount == 0)
            return ResultOf<TEntity>.Failure($"Could not locate items that meet your criteria");

        return new()
        {
            StatusCode = (int)HttpStatusCode.NoContent
        };
    }
}
