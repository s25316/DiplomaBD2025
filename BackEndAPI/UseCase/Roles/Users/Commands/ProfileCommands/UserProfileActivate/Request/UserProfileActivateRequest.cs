using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Request
{
    public class UserProfileActivateRequest : RequestTemplate<UserProfileActivateResponse>
    {
        public required UserProfileActivateCommand Command { get; init; }
    }
}
