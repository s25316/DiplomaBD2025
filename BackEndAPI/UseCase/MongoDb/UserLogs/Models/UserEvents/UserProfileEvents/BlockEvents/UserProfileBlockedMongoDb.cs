// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents.BlockingEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents
{
    public class UserProfileBlockedMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string Message { get; init; }


        // Static Methods
        public static implicit operator UserProfileBlockedMongoDb(PersonBlockedEvent @event)
        {
            return new UserProfileBlockedMongoDb
            {
                UserId = @event.UserId,
                Message = @event.Message,
                TypeId = typeof(UserProfileBlockedMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
