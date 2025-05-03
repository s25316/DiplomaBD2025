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
        // Static Properties
        public static MongoLog MongoLogType { get; protected set; }

        private static Dictionary<MongoLog, Type> _mongoLogsToTypeDictionary = [];
        public static IReadOnlyDictionary<MongoLog, Type> MongoLogsToTypeDictionary => _mongoLogsToTypeDictionary;

        private static Dictionary<Type, MongoLog> _typeToMongoLogsDictionary = [];
        public static IReadOnlyDictionary<Type, MongoLog> TypeToMongoLogsDictionary => _typeToMongoLogsDictionary;

        // Non Static Properties
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
        protected static string ToJson(object item)
        {
            return JsonSerializer.Serialize(item);
        }

        protected static void SetPairMongoLogAndType(MongoLog log, Type type)
        {
            _mongoLogsToTypeDictionary.Add(log, type);
            _typeToMongoLogsDictionary.Add(type, log);
        }

        public static BaseLogMongoDb Map(BsonDocument document)
        {
            var typeIdPropertyName = nameof(BaseLogMongoDb.TypeId);
            if (document.Contains(typeIdPropertyName))
            {
                var typeId = (MongoLog)document[typeIdPropertyName].ToInt32();
                if (MongoLogsToTypeDictionary.TryGetValue(typeId, out var type))
                {
                    var dto = BsonSerializer.Deserialize(document, type);
                    if (dto is BaseLogMongoDb baseLog)
                    {
                        return baseLog;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
        }

    }
}
