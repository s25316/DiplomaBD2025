// Ignore spelling: Mongo
using UseCase.MongoDb.UserLogs.DTOs;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos;

namespace UseCase.MongoDb
{
    public interface IMongoDbService
    {
        Task<UserActivationDataMongoDbDto> GetUserActivationDataAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<UserLoginInDataMongoDbDto> GetUserLoginInDataAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<User2StageDataMongoDbDto> GetUser2StageDataAsync(
            Guid userId,
            string urlSegment,
            string code,
            CancellationToken cancellationToken);
    }
}
