using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize.Request
{
    public class UserProfileResetPasswordAuthorizeRequest : BaseRequest<ProfileCommandResponse>
    {
        public required UserProfileResetPasswordAuthorizeCommand Command { get; init; }
    }
}
