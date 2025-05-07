using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate.Request
{
    public class UserProfileResetPasswordInitiateRequest : RequestTemplate<UserProfileResetPasswordInitiateResponse>
    {
        public required UserProfileResetPasswordInitiateCommand Command { get; init; }
    }
}
