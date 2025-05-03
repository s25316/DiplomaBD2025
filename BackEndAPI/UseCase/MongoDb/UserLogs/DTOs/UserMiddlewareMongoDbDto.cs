// Ignore Spelling: Dto, Mongo, Admin, Middleware
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents;

namespace UseCase.MongoDb.UserLogs.DTOs
{
    public class UserMiddlewareMongoDbDto : BaseUserDataMongoDbDto
    {
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


        private UserAuthorizationLogOutMongoDb? _logOut;
        public UserAuthorizationLogOutMongoDb? LogOut
        {
            get => _logOut;
            set => _logOut ??= value;
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
        public bool IsLogOut => _logOut != null;
    }
}
