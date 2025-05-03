// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.ServiceEvents
{
    public class ApplicationRunMongoDb : BaseLogMongoDb
    {
        // Methods
        public static ApplicationRunMongoDb Prepare()
        {
            return new ApplicationRunMongoDb { TypeId = MongoLog.ApplicationRun };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
