using UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorizationLoginIn;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorizationLoginIn
{
    public class UserAuthorizationLoginInRequest : RequestTemplate<UserAuthorizationLoginInResponse>
    {
        public required UserAuthorizationLoginInCommand Command { get; init; }
    }
}
