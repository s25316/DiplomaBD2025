// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents
{
    public class UserProfileUnBlockedMongoDb : BaseUserLogMongoDb
    {
        // Static Constructor
        static UserProfileUnBlockedMongoDb()
        {
            MongoLogType = MongoLog.UserProfileUnBlocked;
            SetPairMongoLogAndType(MongoLogType, typeof(UserProfileUnBlockedMongoDb));
        }


        // Static Methods
        public static UserProfileUnBlockedMongoDb Prepare(Guid userId)
        {
            return new UserProfileUnBlockedMongoDb
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
