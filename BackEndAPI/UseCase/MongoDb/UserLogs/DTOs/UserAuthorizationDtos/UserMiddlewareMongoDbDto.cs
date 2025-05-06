// Ignore Spelling: Dto, Mongo, Admin, Middleware, Dtos
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos.AbstractClasses;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;

namespace UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos
{
    public class UserMiddlewareMongoDbDto : BaseUserDataMongoDbDto
    {
        // Static Properties
        public static readonly IReadOnlyCollection<int> TypeIds = [
                (int)typeof(UserProfileRemovedMongoDb).GetMongoLog(),
                (int)typeof(UserProfileRestoredMongoDb).GetMongoLog(),

                (int)typeof(UserProfileBlockedMongoDb).GetMongoLog(),
                (int)typeof(UserProfileUnBlockedMongoDb).GetMongoLog(),

                (int)typeof(UserProfileGrantAdminMongoDb).GetMongoLog(),
                (int)typeof(UserProfileRevokeAdminMongoDb).GetMongoLog(),

                (int)typeof(UserAuthorizationLoginInMongoDb).GetMongoLog(),
                (int)typeof(UserAuthorizationRefreshTokenMongoDb).GetMongoLog(),
            ];

        // Non Static Properties
        private UserProfileGrantAdminMongoDb? _grantAdmin;
        public UserProfileGrantAdminMongoDb? GrantAdmin
        {
            get => _grantAdmin;
            set => _grantAdmin ??= value;
        }
        private UserProfileRevokeAdminMongoDb? _revokeAdmin;
        public UserProfileRevokeAdminMongoDb? RevokeAdmin
        {
            get => _revokeAdmin;
            set => _revokeAdmin ??= value;
        }


        private UserAuthorizationRefreshTokenMongoDb? _authorizationData;
        public UserAuthorizationRefreshTokenMongoDb? AuthorizationData
        {
            get => _authorizationData;
            set => _authorizationData ??= value;
        }

        // Computed Properties
        public bool IsAdmin =>
            (
                _grantAdmin != null &&
                _revokeAdmin == null
            ) ||
            (
                _grantAdmin != null &&
                _revokeAdmin != null &&
                _grantAdmin.Created > _revokeAdmin.Created
            );
        public bool HasLogOut => AuthorizationData == null;


        // Static Methods
        public static explicit operator UserMiddlewareMongoDbDto(List<BaseLogMongoDb> logs)
        {
            var dto = new UserMiddlewareMongoDbDto();
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

                if (log is UserProfileGrantAdminMongoDb dtoGrantAdmin)
                {
                    dto.GrantAdmin = dtoGrantAdmin;
                }
                if (log is UserProfileRevokeAdminMongoDb dtoRevokeAdmin)
                {
                    dto.RevokeAdmin = dtoRevokeAdmin;
                }
            }
            return dto;
        }
    }
}
