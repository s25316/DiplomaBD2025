using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate.Request
{
    public class UserProfileCreateRequest : RequestTemplate<ProfileCommandResponse>
    {
        public required UserProfileCreateCommand Command { get; init; }
    }
}
