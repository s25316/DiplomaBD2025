// Ignore Spelling: Dto, Mongo, Json, Admin
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents
{
    public class UserProfileRevokeAdminMongoDb : BaseUserLogMongoDb
    {
        // Static Constructor
        static UserProfileRevokeAdminMongoDb()
        {
            MongoLogType = MongoLog.UserProfileRevokeAdmin;
            SetPairMongoLogAndType(MongoLogType, typeof(UserProfileRevokeAdminMongoDb));
        }


        // Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
