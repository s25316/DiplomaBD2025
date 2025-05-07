// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents.ProfileEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents
{
    public class UserProfileUpdatedPasswordMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Password { get; init; }
        public required string Salt { get; init; }


        // Static Methods
        public static implicit operator UserProfileUpdatedPasswordMongoDb(
            PersonProfileResetPasswordEvent @event)
        {
            return new UserProfileUpdatedPasswordMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserProfileUpdatedPasswordMongoDb).GetMongoLog(),
                Salt = @event.Salt,
                Password = @event.Password,
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
