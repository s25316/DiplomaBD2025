// Ignore Spelling: Mongo, Json
namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents
{
    public class UserProfileInitiatedResetPasswordMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string ResetPasswordUrlSegment { get; init; }


        // Static Methods
        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
