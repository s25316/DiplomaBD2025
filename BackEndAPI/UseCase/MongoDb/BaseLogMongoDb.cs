// Ignore Spelling: Mongo, Json

using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.Models.UserEvents.AuthenticationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents;

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

        private MongoLogs _typeId;
        public required MongoLogs TypeId
        {
            get { return _typeId; }
            init
            {
                Description = value.Description();
                _typeId = value;
            }
        }

        public string Description { get; init; } = null!;
        public DateTime Created { get; init; } = CustomTimeProvider.Now;


        // Methods
        public abstract string ToJson();
        protected static string ToJson(object item)
        {
            return JsonSerializer.Serialize(item);
        }

        public static BaseLogMongoDb Map(BsonDocument document)
        {
            var typeIdPropertyName = nameof(BaseLogMongoDb.TypeId);
            var dictionary = new Dictionary<MongoLogs, Type>()
            {
                // RegistrationEvents
                { MongoLogs.UserProfileCreated,typeof(UserProfileCreatedMongoDb)},
                { MongoLogs.UserProfileActivated, typeof(UserProfileActivatedMongoDb)},

                // BlockEvents
                { MongoLogs.UserProfileBlocked, typeof(UserProfileBlockedMongoDb)},
                { MongoLogs.UserProfileUnBlocked, typeof(UserProfileUnBlockedMongoDb)},

                // RemoveEvents
                { MongoLogs.UserProfileRemoved, typeof(UserProfileRemovedMongoDb)},
                { MongoLogs.UserProfileRestored, typeof(UserProfileRestoredMongoDb)},

                // ResetPasswordEvents
                { MongoLogs.UserProfileUpdatedPassword, typeof(UserProfileUpdatedPasswordMongoDb)},
                { MongoLogs.UserProfileInitiatedResetPassword, typeof(UserProfileInitiatedResetPasswordMongoDb)},
                
                // ResetPasswordEvents
                { MongoLogs.UserProfileGrantAdmin, typeof(UserProfileGrantAdminMongoDb)},
                { MongoLogs.UserProfileRevokeAdmin, typeof(UserProfileRevokeAdminMongoDb)},

                // AuthenticationEvents
                { MongoLogs.UserAuthorization2Stage, typeof(UserAuthorization2StageMongoDb)},
                { MongoLogs.UserAuthorizationLoginIn, typeof(UserAuthorizationLoginInMongoDb)},
                { MongoLogs.UserAuthorizationLogOut, typeof(UserAuthorizationLogOutMongoDb)},
                { MongoLogs.UserAuthorizationRefreshToken, typeof(UserAuthorizationRefreshTokenMongoDb)},
            };

            if (document.Contains(typeIdPropertyName))
            {
                var typeId = document[typeIdPropertyName].ToInt32();
                if (dictionary.TryGetValue((MongoLogs)typeId, out var type))
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
