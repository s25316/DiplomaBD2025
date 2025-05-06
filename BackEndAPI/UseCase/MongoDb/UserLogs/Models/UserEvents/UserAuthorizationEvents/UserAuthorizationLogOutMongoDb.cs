// Ignore Spelling: Mongo, Json, Jwt
using Domain.Features.People.DomainEvents.AuthorizationEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorizationLogOutMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Jwt { get; init; }
        public required string RefreshToken { get; init; }
        public required DateTime RefreshTokenValidTo { get; init; }


        // Static Methods
        public static implicit operator UserAuthorizationLogOutMongoDb(
            PersonAuthorizationLogOutEvent @event)
        {
            return new UserAuthorizationLogOutMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserAuthorizationLogOutMongoDb).GetMongoLog(),
                Jwt = @event.Jwt,
                RefreshToken = @event.RefreshToken,
                RefreshTokenValidTo = @event.RefreshTokenValidTo,
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
