using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Response
{
    public class UserAuthorizationRefreshTokenResponse : ResponseMetaData
    {
        // Property
        public required UserLoginInDataDto? Result { get; init; }


        // Methods
        public static UserAuthorizationRefreshTokenResponse InvalidResponse()
        {
            return new UserAuthorizationRefreshTokenResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Unauthorized,
                Result = null,
            };
        }

        public static UserAuthorizationRefreshTokenResponse ValidResponse(
            UserLoginInDataDto item)
        {
            return new UserAuthorizationRefreshTokenResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Ok,
                Result = item,
            };
        }
    }
}
