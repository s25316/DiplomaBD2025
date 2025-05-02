// Ignore Spelling: Dto, Mongo
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents;

namespace UseCase.MongoDb.UserLogs.DTOs
{
    public class UserActivationMongoDbDto
    {
        // Properties
        private UserProfileCreatedMongoDb? _created;
        public UserProfileCreatedMongoDb? Created
        {
            get => _created;
            set => _created ??= value;
        }

        private UserProfileActivatedMongoDb? _activated;
        public UserProfileActivatedMongoDb? Activated
        {
            get => _activated;
            set => _activated ??= value;
        }

        // Computed Properties
        public bool HasCreated => Created != null;
        public bool HasActivated => Activated != null;
        public string? ActivationUrlSegment => Created?.UrlSegment;


        // Methods
        public static implicit operator UserActivationMongoDbDto(List<BaseLogMongoDb> logs)
        {
            var dto = new UserActivationMongoDbDto();
            foreach (var log in logs)
            {
                if (log is UserProfileCreatedMongoDb dtoCreated)
                {
                    dto.Created = dtoCreated;
                }
                if (log is UserProfileActivatedMongoDb dtoActivated)
                {
                    dto.Activated = dtoActivated;
                }
            }
            return dto;
        }
    }
}
