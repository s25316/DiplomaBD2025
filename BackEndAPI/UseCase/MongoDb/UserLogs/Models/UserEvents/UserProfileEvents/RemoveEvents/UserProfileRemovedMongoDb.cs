// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents.ProfileEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents
{
    public class UserProfileRemovedMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string UrlSegment { get; init; }
        public required DateTime ValidTo { get; init; }
        public bool IsDeactivated { get; init; } = false;


        // Static Methods
        public static implicit operator UserProfileRemovedMongoDb(
            PersonProfileRemovedEvent @event)
        {
            return new UserProfileRemovedMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserProfileRemovedMongoDb).GetMongoLog(),
                UrlSegment = @event.UrlSegment,
                ValidTo = @event.ValidTo,
                Created = @event.Created,
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
