// Ignore Spelling: Mongo, Json

// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents
{
    public class UserProfileRestoredMongoDb : BaseUserLogMongoDb
    {
        // Methods
        public static UserProfileRestoredMongoDb Prepare(Guid userId)
        {
            return new UserProfileRestoredMongoDb
            {
                UserId = userId,
                TypeId = MongoLogs.UserProfileRestored,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
