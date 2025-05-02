// Ignore Spelling: Mongo, Json, Jwt
namespace UseCase.MongoDb.UserLogs.Models.UserEvents.AuthenticationEvents
{
    public class UserAuthorizationLogOutMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Jwt { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime RefreshTokenValidTo { get; init; }


        // Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
