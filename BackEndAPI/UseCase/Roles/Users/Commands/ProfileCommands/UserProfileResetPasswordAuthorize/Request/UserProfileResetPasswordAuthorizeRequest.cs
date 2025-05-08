using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize.Request
{
    public class UserProfileResetPasswordAuthorizeRequest : RequestTemplate<ProfileCommandResponse>
    {
        public required UserProfileResetPasswordAuthorizeCommand Command { get; init; }
    }
}
