using UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.User2StageAuthorization.Request
{
    public class User2StageAuthorizationRequest : BaseRequest<UserAuthorizationResponse>
    {
        public required User2StageAuthorizationCommand Command { get; init; }
    }
}
