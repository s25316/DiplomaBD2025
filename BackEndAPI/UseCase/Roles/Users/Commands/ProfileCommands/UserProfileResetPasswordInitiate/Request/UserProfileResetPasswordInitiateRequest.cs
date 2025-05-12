using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate.Request
{
    public class UserProfileResetPasswordInitiateRequest : BaseRequest<ProfileCommandResponse>
    {
        public required UserProfileResetPasswordInitiateCommand Command { get; init; }
    }
}
