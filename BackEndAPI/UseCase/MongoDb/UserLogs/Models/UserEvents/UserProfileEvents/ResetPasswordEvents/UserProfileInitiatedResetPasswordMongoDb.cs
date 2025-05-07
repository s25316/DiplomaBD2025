// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents.ProfileEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents
{
    public class UserProfileInitiatedResetPasswordMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string UrlSegment { get; init; }
        public required DateTime ValidTo { get; init; }
        public bool IsDeactivated { get; init; } = false;


        // Static Methods
        public static implicit operator UserProfileInitiatedResetPasswordMongoDb(
            PersonProfileInitiateResetPasswordEvent @event)
        {
            return new UserProfileInitiatedResetPasswordMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserProfileInitiatedResetPasswordMongoDb).GetMongoLog(),
                ValidTo = @event.ValidTo,
                UrlSegment = @event.UrlSegment
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
