using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Request
{
    public class UserProfileActivateRequest : BaseRequest<ProfileCommandResponse>
    {
        public required UserProfileActivateCommand Command { get; init; }
    }
}
