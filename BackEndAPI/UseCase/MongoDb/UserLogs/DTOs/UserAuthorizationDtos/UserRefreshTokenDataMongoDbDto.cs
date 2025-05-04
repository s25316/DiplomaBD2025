// Ignore Spelling: Mongo, Dto, Dtos
using Domain.Shared.CustomProviders;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos.AbstractClasses;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;

namespace UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos
{
    public class UserRefreshTokenDataMongoDbDto : BaseUserDataMongoDbDto
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
        private UserAuthorizationLoginInMongoDb? _loginIn;
        public UserAuthorizationLoginInMongoDb? LoginIn
        {
            get => _loginIn;
            set => _loginIn ??= value;
        }

        private UserAuthorizationRefreshTokenMongoDb? _refreshToken;
        public UserAuthorizationRefreshTokenMongoDb? RefreshToken
        {
            get => _refreshToken;
            set => _refreshToken ??= value;
        }
        // Computed Properties
        public bool HasLoggedOut => _loginIn == null && _refreshToken == null;
        public bool HasRefreshTokenExpired =>
            (
                _loginIn == null && _refreshToken == null
            ) ||
            (
                _loginIn != null &&
                _loginIn.RefreshTokenValidTo < CustomTimeProvider.Now
            ) ||
            (
                _refreshToken != null &&
                _refreshToken.RefreshTokenValidTo < CustomTimeProvider.Now
            );



        // Static Methods
        public static explicit operator UserRefreshTokenDataMongoDbDto(List<BaseLogMongoDb> logs)
        {
            var dto = new UserRefreshTokenDataMongoDbDto();
            foreach (var log in logs)
            {
                if (log is UserAuthorizationLoginInMongoDb dtoLoginIn)
                {
                    dto.LoginIn = dtoLoginIn;
                }
                if (log is UserAuthorizationRefreshTokenMongoDb dtoRefreshToken)
                {
                    dto.RefreshToken = dtoRefreshToken;
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
