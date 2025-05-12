using UseCase.Roles.Users.Commands.UserAuthorizationCommands.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Request
{
    public class UserAuthorizationRefreshTokenRequest : BaseRequest<UserAuthorizationResponse>
    {
        public required UserAuthorizationRefreshTokenCommand Command { get; init; }
    }
}
