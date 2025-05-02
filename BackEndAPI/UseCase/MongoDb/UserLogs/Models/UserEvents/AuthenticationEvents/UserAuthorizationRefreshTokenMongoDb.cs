// Ignore Spelling: Mongo, Json
namespace UseCase.MongoDb.UserLogs.Models.UserEvents.AuthenticationEvents
{
    public class UserAuthorizationRefreshTokenMongoDb : UserAuthorizationLogOutMongoDb
    {
        // Properties
        public bool IsDeactivated { get; private set; } = false;


        // Methods
        public void Deactivate() => IsDeactivated = true;

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
