// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorizationRefreshTokenMongoDb : UserAuthorizationLogOutMongoDb
    {
        // Properties
        public bool IsDeactivated { get; private set; } = false;


        // Static Constructor
        static UserAuthorizationRefreshTokenMongoDb()
        {
            MongoLogType = MongoLog.UserAuthorizationRefreshToken;
            SetPairMongoLogAndType(MongoLogType, typeof(UserAuthorizationRefreshTokenMongoDb));
        }


        // Methods
        public void Deactivate() => IsDeactivated = true;

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
