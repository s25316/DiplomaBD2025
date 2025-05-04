// Ignore Spelling: Mongo, Json, Dto, Jwt
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorizationRefreshTokenMongoDb : UserAuthorizationLogOutMongoDb
    {
        // Properties
        public bool IsDeactivated { get; private set; } = false;


        // Static Methods
        public static UserAuthorizationRefreshTokenMongoDb Prepare(
            Guid userId,
            string jwt,
            string refreshToken,
            DateTime refreshTokenValidTo)
        {
            return new UserAuthorizationRefreshTokenMongoDb
            {
                UserId = userId,
                TypeId = typeof(UserAuthorizationRefreshTokenMongoDb).GetMongoLog(),
                Jwt = jwt,
                RefreshToken = refreshToken,
                RefreshTokenValidTo = refreshTokenValidTo,
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
