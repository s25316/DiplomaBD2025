// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents
{
    public class UserProfileUpdatedPasswordMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Password { get; init; }
        public required string Salt { get; init; }


        // Static Constructor
        static UserProfileUpdatedPasswordMongoDb()
        {
            MongoLogType = MongoLog.UserProfileUpdatedPassword;
            SetPairMongoLogAndType(MongoLogType, typeof(UserProfileUpdatedPasswordMongoDb));
        }


        // Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
