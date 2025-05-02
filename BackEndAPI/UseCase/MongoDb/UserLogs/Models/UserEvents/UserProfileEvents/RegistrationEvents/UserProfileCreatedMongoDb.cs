// Ignore Spelling: Mongo, Json

// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents
{
    public class UserProfileCreatedMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string UrlSegment { get; init; }
        public required string Password { get; init; }
        public required string Salt { get; init; }


        // Methods
        public static implicit operator UserProfileCreatedMongoDb(
            PersonProfileCreatedEvent @event)
        {
            return new UserProfileCreatedMongoDb
            {
                UserId = @event.UserId,
                Password = @event.Password,
                Salt = @event.Salt,
                Created = @event.Created,
                TypeId = MongoLogs.UserProfileCreated,
                UrlSegment = @event.UrlSegment,
            };
        }

        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
