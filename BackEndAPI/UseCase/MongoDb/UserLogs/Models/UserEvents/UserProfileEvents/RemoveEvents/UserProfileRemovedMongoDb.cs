// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents
{
    public class UserProfileRemovedMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string RestoreUrlSegment { get; init; }


        // Static Methods
        public static UserProfileRemovedMongoDb Prepare(Guid userId, string urlSegment)
        {
            return new UserProfileRemovedMongoDb
            {
                RestoreUrlSegment = urlSegment,
                UserId = userId,
                TypeId = typeof(UserProfileRemovedMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
