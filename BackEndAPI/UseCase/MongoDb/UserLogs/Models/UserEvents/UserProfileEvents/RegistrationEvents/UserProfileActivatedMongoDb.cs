// Ignore Spelling: Mongo, Json

// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents
{
    public class UserProfileActivatedMongoDb : BaseUserLogMongoDb
    {
        // Methods
        public static implicit operator UserProfileActivatedMongoDb(
            PersonProfileActivatedEvent @event)
        {
            return new UserProfileActivatedMongoDb
            {
                UserId = @event.UserId,
                TypeId = MongoLogs.UserProfileActivated,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
