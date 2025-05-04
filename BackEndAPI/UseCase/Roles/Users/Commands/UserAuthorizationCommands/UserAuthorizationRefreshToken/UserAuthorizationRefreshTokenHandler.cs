// Ignore Spelling: Mongo
using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.Kafka;
using UseCase.MongoDb;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Request;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Response;
using UseCase.Shared.Services.Authentication.Generators;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken
{
    public class UserAuthorizationRefreshTokenHandler : IRequestHandler<UserAuthorizationRefreshTokenRequest, UserAuthorizationRefreshTokenResponse>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IMongoDbService _mongoDbService;
        private readonly IKafkaService _kafka;


        // Constructor
        public UserAuthorizationRefreshTokenHandler(
            IAuthenticationGeneratorService authenticationGenerator,
            IAuthenticationInspectorService authenticationInspector,
            IMongoDbService mongoDbService,
            IKafkaService kafka)
        {
            _authenticationGenerator = authenticationGenerator;
            _authenticationInspector = authenticationInspector;
            _mongoDbService = mongoDbService;
            _kafka = kafka;
        }


        // Methods
        public async Task<UserAuthorizationRefreshTokenResponse> Handle(UserAuthorizationRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var jwtInput = request.Metadata.Authorization;
            if (string.IsNullOrWhiteSpace(jwtInput) ||
                !_authenticationInspector.IsValidJwt(jwtInput, true))
            {
                return InvalidResponse();
            }

            var jwtName = _authenticationInspector.GetClaimsName(jwtInput, true);
            if (string.IsNullOrWhiteSpace(jwtName))
            {
                throw new Exception("Something Changed in Generation JWT");
            }

            var personId = Guid.Parse(jwtName);
            var refreshTokenData = await _mongoDbService
                .GetUserRefreshTokenDataAsync(personId, jwtInput, cancellationToken);

            if ((refreshTokenData.LoginIn == null && refreshTokenData.RefreshToken == null) ||
                refreshTokenData.HasBlocked ||
                refreshTokenData.HasRemoved ||
                refreshTokenData.HasRefreshTokenExpired)
            {
                return InvalidResponse();
            }


            var (jwt, jwtValidTo) = _authenticationGenerator.GenerateJwt(jwtName, []);
            string refreshToken;
            DateTime refreshTokenValidTo;
            if (refreshTokenData.LoginIn != null)
            {
                refreshToken = refreshTokenData.LoginIn.RefreshToken;
                refreshTokenValidTo = refreshTokenData.LoginIn.RefreshTokenValidTo;
            }
            else
            {
                refreshToken = refreshTokenData.RefreshToken.RefreshToken;
                refreshTokenValidTo = refreshTokenData.RefreshToken.RefreshTokenValidTo;
            }

            var mongoDto = UserAuthorizationRefreshTokenMongoDb.Prepare(
                personId,
                jwt,
                refreshToken,
                refreshTokenValidTo);


            await _kafka.SendUserLogAsync(mongoDto, cancellationToken);
            return ValidResponse(new UserLoginInDataDto
            {
                Jwt = jwt,
                JwtValidTo = CustomTimeProvider.ConvertToPoland(jwtValidTo),
                RefreshToken = refreshToken,
                RefreshTokenValidTo = refreshTokenValidTo,
            });
        }

        // Private Static Methods
        public static UserAuthorizationRefreshTokenResponse InvalidResponse()
        {
            return UserAuthorizationRefreshTokenResponse.InvalidResponse();
        }

        public static UserAuthorizationRefreshTokenResponse ValidResponse(
           UserLoginInDataDto item)
        {
            return UserAuthorizationRefreshTokenResponse.ValidResponse(item);
        }
    }
}
