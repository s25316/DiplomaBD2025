// Ignore Spelling: Mongo, Json

using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb
{
    public abstract class BaseLogMongoDb
    {
        // Properties
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        [JsonIgnore]
        public string Id { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime Created { get; init; } = CustomTimeProvider.Now;

        private MongoLog _typeId;
        public required MongoLog TypeId
        {
            get { return _typeId; }
            init
            {
                Description = value.Description();
                _typeId = value;
            }
        }


        // Abstract Methods
        public abstract string ToJson();

        // Static Methods
        protected static string ToJson(object item) => JsonSerializer.Serialize(item);

        public static BaseLogMongoDb Map(BsonDocument document)
        {
            var typeIdPropertyName = nameof(BaseLogMongoDb.TypeId);
            if (document.Contains(typeIdPropertyName))
            {
                var typeId = (MongoLog)document[typeIdPropertyName].ToInt32();
                var type = typeId.GetClassType();

                var dto = BsonSerializer.Deserialize(document, type);
                if (dto is BaseLogMongoDb baseLog)
                {
                    return baseLog;
                }
                else
                {
                    throw new Exception("Impossible");
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
