// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents.BlockingEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents
{
    public class UserProfileUnBlockedMongoDb : BaseUserLogMongoDb
    {
        // Static Methods
        public static implicit operator UserProfileUnBlockedMongoDb(PersonUnBlockedEvent @event)
        {
            return new UserProfileUnBlockedMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserProfileUnBlockedMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
