using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize.Request
{
    public class UserProfileResetPasswordAuthorizeRequest : RequestTemplate<UserProfileResetPasswordAuthorizeResponse>
    {
        public required UserProfileResetPasswordAuthorizeCommand Command { get; init; }
    }
}
