using UseCase.Roles.Users.Commands.UserAuthorizationCommands.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorization2Stage.Request
{
    public class UserAuthorization2StageRequest : BaseRequest<UserAuthorizationResponse>
    {
        public required UserAuthorization2StageCommand Command { get; init; }
    }
}
