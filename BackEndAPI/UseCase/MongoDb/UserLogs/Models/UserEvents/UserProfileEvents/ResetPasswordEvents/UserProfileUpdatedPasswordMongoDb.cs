// Ignore Spelling: Mongo, Json
namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents
{
    public class UserProfileUpdatedPasswordMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Password { get; init; }
        public required string Salt { get; init; }


        // Static Methods
        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
