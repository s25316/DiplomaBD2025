// Ignore Spelling: Mongo, Dtos, Dto
using MongoDB.Bson;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;
using UseCase.Shared.Exceptions;

namespace UseCase.MongoDb.UserLogs.DTOs.UserProfileDtos
{
    public class UserRemovedMongoDbDto
    {
        // Static Properties
        public static readonly int TypeId = (int)typeof(UserProfileRemovedMongoDb).GetMongoLog();

        // Properties
        private UserProfileRemovedMongoDb _removed = null!;
        public UserProfileRemovedMongoDb Removed
        {
            get => _removed;
            set => _removed ??= value;
        }


        // Methods
        public static UserRemovedMongoDbDto Prepare(BsonDocument bsonDocument)
        {
            var log = BaseLogMongoDb.Map(bsonDocument);
            if (log is not UserProfileRemovedMongoDb removedDto)
            {
                throw new UseCaseLayerException("Something changed with mapping");
            }

            return new UserRemovedMongoDbDto
            {
                Removed = removedDto,
            };
        }
    }
}
