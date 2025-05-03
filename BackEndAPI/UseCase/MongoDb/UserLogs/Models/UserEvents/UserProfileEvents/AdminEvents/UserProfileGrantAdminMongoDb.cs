// Ignore Spelling: Dto, Mongo, Json, Admin
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents
{
    public class UserProfileGrantAdminMongoDb : BaseUserLogMongoDb
    {
        // Static Constructor
        static UserProfileGrantAdminMongoDb()
        {
            MongoLogType = MongoLog.UserProfileGrantAdmin;
            SetPairMongoLogAndType(MongoLogType, typeof(UserProfileGrantAdminMongoDb));
        }


        // Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
