using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginIn.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginIn.Request
{
    public class UserAuthorizationLoginInRequest : RequestTemplate<UserAuthorizationLoginInResponse>
    {
        public required UserAuthorizationLoginInCommand Command { get; init; }
    }
}
