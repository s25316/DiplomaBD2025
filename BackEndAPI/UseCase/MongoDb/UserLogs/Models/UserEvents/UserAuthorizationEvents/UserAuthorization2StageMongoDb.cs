// Ignore Spelling: Mongo, Json
using Domain.Features.People.DomainEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents
{
    public class UserAuthorization2StageMongoDb : BaseUserLogMongoDb
    {
        // Properties
        public required string UrlSegment { get; init; }
        public required string Code { get; init; }
        public required DateTime CodeValidTo { get; init; }
        public bool IsDeactivated { get; private set; } = false;


        // Static Methods
        public static explicit operator UserAuthorization2StageMongoDb(
            PersonAuthorization2StageEvent @event)
        {
            return new UserAuthorization2StageMongoDb
            {
                UserId = @event.UserId,
                UrlSegment = @event.UrlSegment,
                Code = @event.Code,
                CodeValidTo = @event.CodeValidTo,
                TypeId = typeof(UserAuthorization2StageMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
