// Ignore Spelling: Mongo, Json, Jwt
namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorizationLogOutMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Jwt { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime RefreshTokenValidTo { get; init; }


        // Static Methods
        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
