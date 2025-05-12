using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginIn.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginIn.Request
{
    public class UserAuthorizationLoginInRequest : BaseRequest<UserAuthorizationLoginInResponse>
    {
        public required UserAuthorizationLoginInCommand Command { get; init; }
    }
}
