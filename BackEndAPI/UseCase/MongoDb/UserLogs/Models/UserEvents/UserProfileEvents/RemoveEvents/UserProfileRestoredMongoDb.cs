// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents.ProfileEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents
{
    public class UserProfileRestoredMongoDb : BaseUserLogMongoDb
    {
        // Static Methods
        public static implicit operator UserProfileRestoredMongoDb(
            PersonProfileRestoredEvent @event)
        {
            return new UserProfileRestoredMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserProfileRestoredMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
