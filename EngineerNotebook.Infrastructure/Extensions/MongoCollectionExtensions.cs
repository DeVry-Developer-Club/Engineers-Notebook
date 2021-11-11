

namespace EngineerNotebook.Infrastructure.Extensions;
internal static class MongoCollectionExtensions
{
    public static async Task<IList<TEntity>> GetAllAsync<TEntity>(this IMongoCollection<TEntity> collection, CancellationToken cancellationToken = default)
    {
        var items = await collection.FindAsync(x => true, cancellationToken: cancellationToken);
        return await items.ToListAsync(cancellationToken: cancellationToken);
    }
}
