// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents
{
    public class UserProfileInitiatedResetPasswordMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string ResetPasswordUrlSegment { get; init; }


        // Static Constructor
        static UserProfileInitiatedResetPasswordMongoDb()
        {
            MongoLogType = MongoLog.UserProfileInitiatedResetPassword;
            SetPairMongoLogAndType(MongoLogType, typeof(UserProfileInitiatedResetPasswordMongoDb));
        }


        // Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
