using UseCase.Roles.Users.Commands.UserAuthorizationCommands.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Request
{
    public class UserAuthorizationRefreshTokenRequest : RequestTemplate<UserAuthorizationResponse>
    {
        public required UserAuthorizationRefreshTokenCommand Command { get; init; }
    }
}
