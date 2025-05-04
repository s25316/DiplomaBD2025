// Ignore Spelling: Dto, Mongo, Admin, Middleware
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos.AbstractClasses;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;

namespace UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos
{
    public class UserMiddlewareDataMongoDbDto : BaseUserDataMongoDbDto
    {
        // Static Properties
        public static readonly IReadOnlyCollection<int> TypeIds = [
                (int)typeof(UserProfileRemovedMongoDb).GetMongoLog(),
                (int)typeof(UserProfileRestoredMongoDb).GetMongoLog(),

                (int)typeof(UserProfileBlockedMongoDb).GetMongoLog(),
                (int)typeof(UserProfileUnBlockedMongoDb).GetMongoLog(),

                (int)typeof(UserProfileGrantAdminMongoDb).GetMongoLog(),
                (int)typeof(UserProfileRevokeAdminMongoDb).GetMongoLog(),

                // Add 
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


        private UserAuthorizationLogOutMongoDb? _logOut;
        public UserAuthorizationLogOutMongoDb? LogOut
        {
            get => _logOut;
            set => _logOut ??= value;
        }

        // Computed Properties
        public bool IsAdmin =>

                _grantAdmin != null &&
                _revokeAdmin == null
             ||

                _grantAdmin != null &&
                _revokeAdmin != null &&
                _grantAdmin.Created > _revokeAdmin.Created
            ;
        public bool IsLogOut => _logOut != null;
    }
}
