// Ignore Spelling: Mongo, Json

// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents
{
    public class UserProfileBlockedMongoDb : BaseUserLogMongoDb
    {
        // Methods
        public static UserProfileBlockedMongoDb Prepare(Guid userId)
        {
            return new UserProfileBlockedMongoDb
            {
                UserId = userId,
                TypeId = MongoLogs.UserProfileBlocked,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
