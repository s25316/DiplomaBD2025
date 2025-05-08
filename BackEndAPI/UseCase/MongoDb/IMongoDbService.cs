// Ignore spelling: Mongo
using UseCase.MongoDb.UserLogs.DTOs;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos;
using UseCase.MongoDb.UserLogs.DTOs.UserProfileDtos;

namespace UseCase.MongoDb
{
    public interface IMongoDbService
    {
        Task<UserActivationDataMongoDbDto> GetUserActivationDataAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<UserLoginInMongoDbDto> GetUserLoginInDataAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<User2StageMongoDbDto> GetUser2StageDataAsync(
            Guid userId,
            string urlSegment,
            string code,
            CancellationToken cancellationToken);

        Task<UserRefreshTokenMongoDbDto> GetUserRefreshTokenDataAsync(
            Guid userId,
            string jwt,
            string refreshToken,
            CancellationToken cancellationToken);

        Task<UserLogOutMongoDbDto> GetUserLogOutDataAsync(
            Guid userId,
            string jwt,
            CancellationToken cancellationToken);

        Task<UserMiddlewareMongoDbDto> GetUserMiddlewareDataAsync(
            Guid userId,
            string jwt,
            CancellationToken cancellationToken);

        Task<UserResetPasswordInitiationMongoDbDto> GeUserResetPasswordInitiationAsync(
            Guid userId,
            string urlSegment,
            CancellationToken cancellationToken);

        Task<UserRemovedMongoDbDto> GeUserRemovedAsync(
            Guid userId,
            string urlSegment,
            CancellationToken cancellationToken);
    }
}
