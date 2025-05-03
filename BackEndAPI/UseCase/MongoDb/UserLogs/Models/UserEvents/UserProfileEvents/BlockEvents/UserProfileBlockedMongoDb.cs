// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents
{
    public class UserProfileBlockedMongoDb : BaseUserLogMongoDb
    {
        // Static Constructor
        static UserProfileBlockedMongoDb()
        {
            MongoLogType = MongoLog.UserProfileBlocked;
            SetPairMongoLogAndType(MongoLogType, typeof(UserProfileBlockedMongoDb));
        }


        // Static Methods
        public static UserProfileBlockedMongoDb Prepare(Guid userId)
        {
            return new UserProfileBlockedMongoDb
            {
                UserId = userId,
                TypeId = MongoLogType,
            };
        }

        // Non Static Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
