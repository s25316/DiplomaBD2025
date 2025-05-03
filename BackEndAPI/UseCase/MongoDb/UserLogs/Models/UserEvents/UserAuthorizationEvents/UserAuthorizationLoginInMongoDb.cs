// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorizationLoginInMongoDb : UserAuthorizationRefreshTokenMongoDb
    {
        // Static Constructor
        static UserAuthorizationLoginInMongoDb()
        {
            MongoLogType = MongoLog.UserAuthorizationLoginIn;
            SetPairMongoLogAndType(MongoLogType, typeof(UserAuthorizationLoginInMongoDb));
        }


        // Static Methods
        public static explicit operator UserAuthorizationLoginInMongoDb(
            PersonAuthorizationLoginInEvent @event)
        {
            return new UserAuthorizationLoginInMongoDb
            {
                UserId = @event.UserId,
                TypeId = MongoLogType,
                Jwt = @event.Jwt,
                RefreshToken = @event.RefreshToken,
                RefreshTokenValidTo = @event.RefreshTokenValidTo,
            };
        }

        // Non Static Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
