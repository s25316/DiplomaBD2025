using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate.Request
{
    public class UserProfileCreateRequest : RequestTemplate<UserProfileCreateResponse>
    {
        public required UserProfileCreateCommand Command { get; init; }
    }
}
