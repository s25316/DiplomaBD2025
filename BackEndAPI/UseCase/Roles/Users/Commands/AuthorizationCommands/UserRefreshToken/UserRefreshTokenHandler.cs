// Ignore Spelling: Mongo
using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.MongoDb;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses;
using UseCase.Roles.Users.Commands.AuthorizationCommands.UserRefreshToken.Request;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Interfaces;
using UseCase.Shared.Services.Authentication.Generators;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserRefreshToken
{
    public class UserRefreshTokenHandler : IRequestHandler<UserRefreshTokenRequest, UserAuthorizationResponse>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IMongoDbService _mongoDbService;
        private readonly IKafkaService _kafka;


        // Constructor
        public UserRefreshTokenHandler(
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
        public async Task<UserAuthorizationResponse> Handle(UserRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var jwtInput = request.Metadata.Authorization;
            if (string.IsNullOrWhiteSpace(jwtInput) ||
                !_authenticationInspector.IsValidJwt(jwtInput, true))
            {
                return InvalidResponse();
            }

            var jwtName = _authenticationInspector.GetClaimsName(jwtInput, true);
            if (string.IsNullOrWhiteSpace(jwtName) ||
                !Guid.TryParse(jwtName, out var personId))
            {
                throw new UseCaseLayerException("Something Changed in Generation JWT");
            }

            // MongoDB Part
            var refreshTokenData = await _mongoDbService
                .GetUserRefreshTokenDataAsync(
                personId,
                jwtInput,
                request.Command.RefreshToken,
                cancellationToken);

            if (refreshTokenData.AuthorizationData == null ||
                refreshTokenData.HasBlocked ||
                refreshTokenData.HasRemoved ||
                refreshTokenData.HasRefreshTokenExpired)
            {
                return InvalidResponse();
            }


            var (jwt, jwtValidTo) = _authenticationGenerator.GenerateJwt(jwtName, []);
            var loginInData = new UserAuthorizationDataDto
            {
                Jwt = jwt,
                JwtValidTo = CustomTimeProvider.ConvertToPoland(jwtValidTo),
                RefreshToken = refreshTokenData.AuthorizationData.RefreshToken,
                RefreshTokenValidTo = refreshTokenData.AuthorizationData.RefreshTokenValidTo,
            };
            var mongoDto = UserAuthorizationRefreshTokenMongoDb.Prepare(
                personId,
                jwt,
                loginInData.RefreshToken,
                loginInData.RefreshTokenValidTo);


            await _kafka.SendUserLogAsync(mongoDto, cancellationToken);
            return ValidResponse(loginInData);
        }

        // Private Static Methods
        private static UserAuthorizationResponse InvalidResponse()
        {
            return UserAuthorizationResponse.InvalidResponse();
        }

        private static UserAuthorizationResponse ValidResponse(
           UserAuthorizationDataDto item)
        {
            return UserAuthorizationResponse.ValidResponse(item);
        }
    }
}
