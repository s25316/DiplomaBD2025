// Ignore spelling: Mongo
using UseCase.MongoDb.UserLogs.DTOs;

namespace UseCase.MongoDb
{
    public interface IMongoDbService
    {
        Task<UserActivationMongoDbDto> GetUserProfileActivationData(
            Guid userId,
            CancellationToken cancellationToken);

        Task<UserLoginInMongoDbDto> GetLoginInDataAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<UserAuthorization2StageMongoDbDto> Get2StageAuthorizationAsync(
            Guid userId,
            string urlSegment,
            string code,
            CancellationToken cancellationToken);
    }
}
