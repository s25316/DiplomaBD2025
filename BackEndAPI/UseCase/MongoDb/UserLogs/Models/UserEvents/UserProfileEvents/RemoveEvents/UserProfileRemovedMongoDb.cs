// Ignore Spelling: Mongo, Json

// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents
{
    public class UserProfileRemovedMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string RestoreUrlSegment { get; init; }


        // Methods
        public static UserProfileRemovedMongoDb Prepare(Guid userId, string urlSegment)
        {
            return new UserProfileRemovedMongoDb
            {
                RestoreUrlSegment = urlSegment,
                UserId = userId,
                TypeId = MongoLogs.UserProfileRemoved,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
