// Ignore Spelling: Mongo, Json

// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents
{
    public class UserProfileUpdatedPasswordMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Password { get; init; }
        public required string Salt { get; init; }


        // Methods
        public static UserProfileUpdatedPasswordMongoDb Prepare(
            Guid userId,
            string password,
            string salt)
        {
            return new UserProfileUpdatedPasswordMongoDb
            {
                Password = password,
                Salt = salt,
                UserId = userId,
                TypeId = MongoLogs.UserProfileUpdatedPassword,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
