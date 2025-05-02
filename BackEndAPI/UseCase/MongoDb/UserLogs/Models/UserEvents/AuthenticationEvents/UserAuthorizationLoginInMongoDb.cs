// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.AuthenticationEvents
{
    public class UserAuthorizationLoginInMongoDb : UserAuthorizationRefreshTokenMongoDb
    {
        // Methods
        public static explicit operator UserAuthorizationLoginInMongoDb(
            PersonAuthorizationLoginInEvent @event)
        {
            return new UserAuthorizationLoginInMongoDb
            {
                UserId = @event.UserId,
                TypeId = Enums.MongoLogs.UserAuthorizationLoginIn,
                Jwt = @event.Jwt,
                RefreshToken = @event.RefreshToken,
                RefreshTokenValidTo = @event.RefreshTokenValidTo,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
