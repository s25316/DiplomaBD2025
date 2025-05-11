using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetRegularData.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetRegularData.Request
{
    public class UserProfileSetRegularDataRequest : RequestTemplate<UserProfileSetRegularDataResponse>
    {
        public required UserProfileSetRegularDataCommand Command { get; init; }
    }
}
