// Ignore Spelling: Mongo, Json
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.ServiceEvents
{
    public class ApplicationRunMongoDb : BaseLogMongoDb
    {
        // Static Methods
        public static ApplicationRunMongoDb Prepare()
        {
            return new ApplicationRunMongoDb
            {
                TypeId = typeof(ApplicationRunMongoDb).GetMongoLog()
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
