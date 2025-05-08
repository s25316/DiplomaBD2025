using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Request
{
    public class UserProfileActivateRequest : RequestTemplate<ProfileCommandResponse>
    {
        public required UserProfileActivateCommand Command { get; init; }
    }
}
