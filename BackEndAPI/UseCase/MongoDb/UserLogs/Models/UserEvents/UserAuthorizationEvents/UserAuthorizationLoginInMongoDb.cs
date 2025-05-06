// Ignore Spelling: Mongo, Json, GetMongoLog
using Domain.Features.People.DomainEvents.AuthorizationEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorizationLoginInMongoDb : UserAuthorizationRefreshTokenMongoDb
    {
        // Static Methods
        public static explicit operator UserAuthorizationLoginInMongoDb(
            PersonAuthorizationLoginInEvent @event)
        {
            return new UserAuthorizationLoginInMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserAuthorizationLoginInMongoDb).GetMongoLog(),
                Jwt = @event.Jwt,
                RefreshToken = @event.RefreshToken,
                RefreshTokenValidTo = @event.RefreshTokenValidTo,
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
