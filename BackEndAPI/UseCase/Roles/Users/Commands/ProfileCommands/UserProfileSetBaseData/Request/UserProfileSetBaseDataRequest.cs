using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Request
{
    public class UserProfileSetBaseDataRequest : RequestTemplate<UserProfileSetBaseDataResponse>
    {
        public required UserProfileSetBaseDataCommand Command { get; init; }
    }
}
