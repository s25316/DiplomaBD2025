// Ignore Spelling: Mongo, Json

// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents
{
    public class UserProfileUnBlockedMongoDb : BaseUserLogMongoDb
    {
        // Methods
        public static UserProfileUnBlockedMongoDb Prepare(Guid userId)
        {
            return new UserProfileUnBlockedMongoDb
            {
                UserId = userId,
                TypeId = MongoLogs.UserProfileUnBlocked,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
