// Ignore Spelling: Dto, Dtos, Mongo
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;

namespace UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos.AbstractClasses
{
    public abstract class BaseUserDataMongoDbDto
    {
        // Properties
        private UserProfileRemovedMongoDb? _removed;
        public UserProfileRemovedMongoDb? Removed
        {
            get => _removed;
            set => _removed ??= value;
        }
        private UserProfileRestoredMongoDb? _restored;
        public UserProfileRestoredMongoDb? Restored
        {
            get => _restored;
            set => _restored ??= value;
        }


        private UserProfileBlockedMongoDb? _blocked;
        public UserProfileBlockedMongoDb? Blocked
        {
            get => _blocked;
            set => _blocked ??= value;
        }
        private UserProfileUnBlockedMongoDb? _unBlocked;
        public UserProfileUnBlockedMongoDb? UnBlocked
        {
            get => _unBlocked;
            set => _unBlocked ??= value;
        }

        // Computed Properties
        public bool HasRemoved =>
            (
                _removed != null &&
               _restored == null
            ) || (
                _removed != null &&
               _restored != null &&
               _removed.Created > _restored.Created
            );
        public bool HasBlocked =>
            (
                _blocked != null &&
                _unBlocked == null
            ) || (
                _blocked != null &&
                _unBlocked != null &&
                _blocked.Created > _unBlocked.Created
            );
    }
}
