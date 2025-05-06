// Ignore Spelling: Dto, Mongo, DTOs
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;

namespace UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos
{
    public class UserLogOutMongoDbDto
    {
        // Static Properties
        public static readonly IReadOnlyCollection<int> TypeIds = [
                (int)typeof(UserAuthorizationLogOutMongoDb).GetMongoLog(),

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

        private UserAuthorizationLogOutMongoDb? _logOut;
        public UserAuthorizationLogOutMongoDb? LogOut
        {
            get => _logOut;
            set => _logOut ??= value;
        }

        // Computed Properties
        public bool HasLogOut => _logOut != null;


        // Static Methods
        public static explicit operator UserLogOutMongoDbDto(List<BaseLogMongoDb> logs)
        {
            var dto = new UserLogOutMongoDbDto();
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
                if (log is UserAuthorizationLogOutMongoDb dtoLogOut &&
                    dtoLogOut.TypeId == typeof(UserAuthorizationLogOutMongoDb).GetMongoLog())
                {
                    dto.LogOut = dtoLogOut;
                }
            }
            return dto;
        }
    }
}
