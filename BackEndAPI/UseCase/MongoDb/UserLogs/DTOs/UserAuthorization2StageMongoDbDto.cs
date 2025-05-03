// Ignore Spelling: Mongo, Dto
using Domain.Shared.CustomProviders;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;

namespace UseCase.MongoDb.UserLogs.DTOs
{
    public class UserAuthorization2StageMongoDbDto
    {
        // Static Properties
        public static readonly int TypeId = (int)UserAuthorization2StageMongoDb.MongoLogType;

        // Non Static Properties
        public UserAuthorization2StageMongoDb? Item { get; init; }

        // Computed Properties
        public bool HasValue => Item != null;
        public bool HasExpired => Item == null || Item.CodeValidTo < CustomTimeProvider.Now;
        public Guid? UserId => Item?.UserId;
    }
}
