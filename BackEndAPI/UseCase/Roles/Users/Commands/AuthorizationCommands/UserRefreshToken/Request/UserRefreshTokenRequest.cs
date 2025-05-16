using UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserRefreshToken.Request
{
    public class UserRefreshTokenRequest : BaseRequest<UserAuthorizationResponse>
    {
        public required UserRefreshTokenCommand Command { get; init; }
    }
}
