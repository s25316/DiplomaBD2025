// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents.ProfileEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents
{
    public class UserProfileActivatedMongoDb : BaseUserLogMongoDb
    {
        // Static  Methods
        public static implicit operator UserProfileActivatedMongoDb(
            PersonProfileActivatedEvent @event)
        {
            return new UserProfileActivatedMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserProfileActivatedMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
