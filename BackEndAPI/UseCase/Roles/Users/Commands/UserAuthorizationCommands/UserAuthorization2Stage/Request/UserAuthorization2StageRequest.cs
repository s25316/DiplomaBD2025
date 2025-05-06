using UseCase.Roles.Users.Commands.UserAuthorizationCommands.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorization2Stage.Request
{
    public class UserAuthorization2StageRequest : RequestTemplate<UserAuthorizationResponse>
    {
        public required UserAuthorization2StageCommand Command { get; init; }
    }
}
