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


        // Static Methods
        public static implicit operator UserProfileCreatedMongoDb(
            PersonProfileCreatedEvent @event)
        {
            return new UserProfileCreatedMongoDb
            {
                UserId = @event.UserId,
                Password = @event.Password,
                Salt = @event.Salt,
                Created = @event.Created,
                TypeId = typeof(UserProfileCreatedMongoDb).GetMongoLog(),
                UrlSegment = @event.UrlSegment,
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
