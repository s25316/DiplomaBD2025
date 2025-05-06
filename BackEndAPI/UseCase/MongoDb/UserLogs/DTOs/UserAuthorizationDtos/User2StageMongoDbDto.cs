// Ignore Spelling: Mongo, Dto, Dtos
using Domain.Shared.CustomProviders;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;

namespace UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos
{
    public class User2StageMongoDbDto
    {
        // Static Properties
        public static readonly int TypeId = (int)typeof(UserAuthorization2StageMongoDb).GetMongoLog();

        // Non Static Properties
        public UserAuthorization2StageMongoDb? Item { get; init; }

        // Computed Properties
        public bool HasValue => Item != null;
        public bool HasExpired => Item == null || Item.CodeValidTo < CustomTimeProvider.Now;
        public Guid? UserId => Item?.UserId;


        // Static Methods
        public static User2StageMongoDbDto PrepareEmpty()
        {
            return new User2StageMongoDbDto { };
        }

        public static User2StageMongoDbDto PrepareNotEmpty(
            UserAuthorization2StageMongoDb item)
        {
            return new User2StageMongoDbDto
            {
                Item = item,
            };
        }
    }
}
