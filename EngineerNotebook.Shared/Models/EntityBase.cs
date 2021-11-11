using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EngineerNotebook.Shared.Models;
public abstract class EntityBase : IEntityWithTypedId<string>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
}
