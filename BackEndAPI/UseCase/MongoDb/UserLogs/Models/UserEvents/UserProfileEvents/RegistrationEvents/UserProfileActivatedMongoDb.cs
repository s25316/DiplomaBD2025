// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents
{
    public class UserProfileActivatedMongoDb : BaseUserLogMongoDb
    {
        // Static Constructor
        static UserProfileActivatedMongoDb()
        {
            MongoLogType = MongoLog.UserProfileActivated;
            SetPairMongoLogAndType(MongoLogType, typeof(UserProfileActivatedMongoDb));
        }


        // Static  Methods
        public static implicit operator UserProfileActivatedMongoDb(
            PersonProfileActivatedEvent @event)
        {
            return new UserProfileActivatedMongoDb
            {
                UserId = @event.UserId,
                TypeId = MongoLogType,
            };
        }

        // Non Static Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
