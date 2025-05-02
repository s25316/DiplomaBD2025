// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents
{
    public class UserProfileInitiatedResetPasswordMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string ResetPasswordUrlSegment { get; init; }


        // Methods
        public static UserProfileInitiatedResetPasswordMongoDb Prepare(
            Guid userId,
            string urlSegment)
        {
            return new UserProfileInitiatedResetPasswordMongoDb
            {
                ResetPasswordUrlSegment = urlSegment,
                UserId = userId,
                TypeId = MongoLogs.UserProfileInitiatedResetPassword,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
