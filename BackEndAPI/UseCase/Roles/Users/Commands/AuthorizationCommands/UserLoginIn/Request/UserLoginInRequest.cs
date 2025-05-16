using UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Request
{
    public class UserLoginInRequest : BaseRequest<UserLoginInResponse>
    {
        public required UserLoginInCommand Command { get; init; }
    }
}
