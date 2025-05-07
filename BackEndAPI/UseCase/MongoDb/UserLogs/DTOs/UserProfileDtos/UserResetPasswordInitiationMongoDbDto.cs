// Ignore Spelling: Mongo, Dto
using Domain.Shared.CustomProviders;
using MongoDB.Bson;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents;
using UseCase.Shared.Exceptions;

namespace UseCase.MongoDb.UserLogs.DTOs.UserProfileDtos
{
    public class UserResetPasswordInitiationMongoDbDto
    {
        // Static Properties

        public static readonly int TypeId = (int)typeof(UserProfileInitiatedResetPasswordMongoDb).GetMongoLog();

        // Non Static Properties
        private UserProfileInitiatedResetPasswordMongoDb _initiation = null!;
        public UserProfileInitiatedResetPasswordMongoDb Initiation
        {
            get => _initiation;
            set => _initiation ??= value;
        }

        // Computed Properties
        public bool IsDeactivated => Initiation == null ||
            Initiation.IsDeactivated;
        public bool IsValid =>
            Initiation != null &&
            Initiation.ValidTo >= CustomTimeProvider.Now;


        // Methods
        public static UserResetPasswordInitiationMongoDbDto Prepare(BsonDocument bsonDocument)
        {
            var log = BaseLogMongoDb.Map(bsonDocument);
            if (log is not UserProfileInitiatedResetPasswordMongoDb initiationDto)
            {
                throw new UseCaseLayerException("Something changed in mapping");
            }

            return new UserResetPasswordInitiationMongoDbDto
            {
                Initiation = initiationDto,
            };
        }
    }
}
