// Ignore Spelling: Mongo, Dto, Dtos
using Domain.Shared.CustomProviders;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos.AbstractClasses;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;

namespace UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos
{
    public class UserRefreshTokenMongoDbDto : BaseUserDataMongoDbDto
    {
        // Static Properties
        public static readonly IReadOnlyCollection<int> TypeIds = [
                (int)typeof(UserProfileRemovedMongoDb).GetMongoLog(),
                (int)typeof(UserProfileRestoredMongoDb).GetMongoLog(),

                (int)typeof(UserProfileBlockedMongoDb).GetMongoLog(),
                (int)typeof(UserProfileUnBlockedMongoDb).GetMongoLog(),

                (int)typeof(UserAuthorizationLoginInMongoDb).GetMongoLog(),
                (int)typeof(UserAuthorizationRefreshTokenMongoDb).GetMongoLog(),
            ];

        // Non Static Properties
        private UserAuthorizationRefreshTokenMongoDb? _authorizationData;
        public UserAuthorizationRefreshTokenMongoDb? AuthorizationData
        {
            get => _authorizationData;
            set => _authorizationData ??= value;
        }

        // Computed Properties
        public bool HasRefreshTokenExpired =>
            _authorizationData == null ||
            _authorizationData.RefreshTokenValidTo < CustomTimeProvider.Now;



        // Static Methods
        public static explicit operator UserRefreshTokenMongoDbDto(List<BaseLogMongoDb> logs)
        {
            var dto = new UserRefreshTokenMongoDbDto();
            foreach (var log in logs)
            {
                if (log is UserAuthorizationLoginInMongoDb dtoLoginIn)
                {
                    dto.AuthorizationData = dtoLoginIn;
                }
                if (log is UserAuthorizationRefreshTokenMongoDb dtoRefreshToken)
                {
                    dto.AuthorizationData = dtoRefreshToken;
                }

                // BaseUserDataDto Data 
                if (log is UserProfileBlockedMongoDb dtoBlocked)
                {
                    dto.Blocked = dtoBlocked;
                }
                if (log is UserProfileUnBlockedMongoDb dtoUnBlocked)
                {
                    dto.UnBlocked = dtoUnBlocked;
                }

                if (log is UserProfileRemovedMongoDb dtoRemoved)
                {
                    dto.Removed = dtoRemoved;
                }
                if (log is UserProfileRestoredMongoDb dtoRestored)
                {
                    dto.Restored = dtoRestored;
                }
            }
            return dto;
        }
    }
}
