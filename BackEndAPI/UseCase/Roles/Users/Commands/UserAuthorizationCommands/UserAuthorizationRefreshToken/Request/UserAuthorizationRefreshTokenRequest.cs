using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Request
{
    public class UserAuthorizationRefreshTokenRequest : RequestTemplate<UserAuthorizationRefreshTokenResponse>
    {
        public required UserAuthorizationRefreshTokenCommand Command { get; init; }
    }
}
