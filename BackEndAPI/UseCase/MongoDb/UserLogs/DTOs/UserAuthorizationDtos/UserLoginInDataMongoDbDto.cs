// Ignore Spelling: Dto, Dtos, Mongo, Admin
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos.AbstractClasses;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;

namespace UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos
{
    public class UserLoginInDataMongoDbDto : BaseUserDataMongoDbDto
    {
        // Static Properties
        public static readonly IReadOnlyCollection<int> TypeIds = [
                (int)typeof(UserProfileRemovedMongoDb).GetMongoLog(),
                (int)typeof(UserProfileRestoredMongoDb).GetMongoLog(),

                (int)typeof(UserProfileBlockedMongoDb).GetMongoLog(),
                (int)typeof(UserProfileUnBlockedMongoDb).GetMongoLog(),

                (int)typeof(UserProfileActivatedMongoDb).GetMongoLog(),
                (int)typeof(UserAuthorization2StageMongoDb).GetMongoLog(),
            ];

        // Non Static Properties
        private UserProfileActivatedMongoDb? _activated;
        public UserProfileActivatedMongoDb? Activated
        {
            get => _activated;
            set => _activated ??= value;
        }

        private UserAuthorization2StageMongoDb? _handStage;
        public UserAuthorization2StageMongoDb? HandStage
        {
            get => _handStage;
            set => _handStage ??= value;
        }

        // Computed Properties
        public bool HasActivated => _activated != null;
        public DateTime? LastHandStage => _handStage?.Created;


        // Static Methods
        public static explicit operator UserLoginInDataMongoDbDto(List<BaseLogMongoDb> logs)
        {
            var dto = new UserLoginInDataMongoDbDto();
            foreach (var log in logs)
            {
                if (log is UserProfileActivatedMongoDb dtoActivated)
                {
                    dto.Activated = dtoActivated;
                }
                if (log is UserAuthorization2StageMongoDb dtoHandStage)
                {
                    dto.HandStage = dtoHandStage;
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
