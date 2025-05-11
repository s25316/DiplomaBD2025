using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileUpdateLogin.Request
{
    public class UserProfileUpdateLoginRequest : RequestTemplate<ProfileCommandResponse>
    {
        public required UserProfileUpdateLoginCommand Command { get; init; }
    }
}
