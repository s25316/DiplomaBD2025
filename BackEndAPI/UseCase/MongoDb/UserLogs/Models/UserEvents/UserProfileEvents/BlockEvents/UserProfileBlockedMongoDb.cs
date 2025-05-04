// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents
{
    public class UserProfileBlockedMongoDb : BaseUserLogMongoDb
    {
        // Static Methods
        public static UserProfileBlockedMongoDb Prepare(Guid userId)
        {
            return new UserProfileBlockedMongoDb
            {
                UserId = userId,
                TypeId = typeof(UserProfileBlockedMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
