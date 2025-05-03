// Ignore Spelling: Mongo, Json, Jwt
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorizationLogOutMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Jwt { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime RefreshTokenValidTo { get; init; }


        // Static Constructor
        static UserAuthorizationLogOutMongoDb()
        {
            MongoLogType = MongoLog.UserAuthorizationLogOut;
            SetPairMongoLogAndType(MongoLogType, typeof(UserAuthorizationLogOutMongoDb));
        }


        // Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
