// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents
{
    public class UserProfileUnBlockedMongoDb : BaseUserLogMongoDb
    {
        // Static Methods
        public static UserProfileUnBlockedMongoDb Prepare(Guid userId)
        {
            return new UserProfileUnBlockedMongoDb
            {
                UserId = userId,
                TypeId = typeof(UserProfileUnBlockedMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
