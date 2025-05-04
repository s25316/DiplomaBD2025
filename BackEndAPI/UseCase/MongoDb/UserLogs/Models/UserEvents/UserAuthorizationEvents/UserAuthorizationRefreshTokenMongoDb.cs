// Ignore Spelling: Mongo, Json
namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorizationRefreshTokenMongoDb : UserAuthorizationLogOutMongoDb
    {
        // Properties
        public bool IsDeactivated { get; private set; } = false;


        // Static Methods
        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
