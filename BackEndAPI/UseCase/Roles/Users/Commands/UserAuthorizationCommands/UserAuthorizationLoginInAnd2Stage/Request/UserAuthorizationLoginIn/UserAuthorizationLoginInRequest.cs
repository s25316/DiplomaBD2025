using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorizationLoginIn;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorizationLoginIn
{
    public class UserAuthorizationLoginInRequest : RequestTemplate<UserAuthorizationLoginInResponse>
    {
        public required UserAuthorizationLoginInCommand Command { get; init; }
    }
}
