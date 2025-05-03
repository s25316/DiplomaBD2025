// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents
{
    public class UserProfileRestoredMongoDb : BaseUserLogMongoDb
    {
        // Static Constructor
        static UserProfileRestoredMongoDb()
        {
            MongoLogType = MongoLog.UserProfileRestored;
            SetPairMongoLogAndType(MongoLogType, typeof(UserProfileRestoredMongoDb));
        }


        // Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
