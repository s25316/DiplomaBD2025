// Ignore Spelling: Mongo
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UseCase.MongoDb.UserLogs
{
    public abstract class BaseUserLogMongoDb : BaseLogMongoDb
    {
        [BsonRepresentation(BsonType.String)]
        public required Guid UserId { get; init; }
    }
}
