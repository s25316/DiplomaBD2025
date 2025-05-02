using UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorization2Stage;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorization2Stage
{
    public class UserAuthorization2StageRequest : RequestTemplate<UserAuthorization2StageResponse>
    {
        public required UserAuthorization2StageCommand Command { get; init; }
    }
}
