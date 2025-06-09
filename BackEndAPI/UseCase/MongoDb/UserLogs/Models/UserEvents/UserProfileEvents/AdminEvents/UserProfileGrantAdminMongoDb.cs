// Ignore Spelling: Dto, Mongo, Json, Admin
using Domain.Features.People.DomainEvents.AdministrationEvents;
using UseCase.MongoDb.Enums;

namespace UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents
{
    public class UserProfileGrantAdminMongoDb : BaseUserLogMongoDb
    {
        // Static Methods
        public static implicit operator UserProfileGrantAdminMongoDb(PersonAdministrationGrantEvent @event)
        {
            return new UserProfileGrantAdminMongoDb
            {
                UserId = @event.UserId,
                TypeId = typeof(UserProfileGrantAdminMongoDb).GetMongoLog(),
            };
        }

        // Non Static Methods
        public override string ToJson() => ToJson(this);
    }
}
