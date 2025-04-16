// Ignore spelling: Dto, Mongo
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UseCase.MongoDb.Models.UserActions
{
    public class UserActionDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; } = null!;

        [BsonElement("userId")]
        public Guid UserId { get; init; }

        [BsonElement("created")]
        public DateTime Created { get; init; }

        [BsonElement("actionId")]
        public UserAction ActionId { get; init; }

        [BsonElement("publicInformation")]
        public string PublicInformation { get; init; } = null!;

        [BsonElement("privateInformation")]
        public string PrivateInformation { get; init; } = null!;
    }
}
