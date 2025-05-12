using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileUpdateLogin.Request
{
    public class UserProfileUpdateLoginRequest : BaseRequest<ProfileCommandResponse>
    {
        public required UserProfileUpdateLoginCommand Command { get; init; }
    }
}
