// Ignore Spelling: Dto, Mongo, Json, Admin
namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents
{
    public class UserProfileRevokeAdminMongoDb : BaseUserLogMongoDb
    {
        // Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
