// Ignore Spelling: Dto, Mongo, Json, Admin
using Domain.Features.People.DomainEvents.AdministrationEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents
{
    public class UserProfileRevokeAdminMongoDb : BaseUserLogMongoDb
    {

        // Static Methods
        public static implicit operator UserProfileRevokeAdminMongoDb(PersonAdministrationRevokeEvent @event)
        {
            return new UserProfileRevokeAdminMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserProfileRevokeAdminMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
