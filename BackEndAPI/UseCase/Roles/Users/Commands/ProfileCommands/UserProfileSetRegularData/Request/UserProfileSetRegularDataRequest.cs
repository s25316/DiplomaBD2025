using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetRegularData.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetRegularData.Request
{
    public class UserProfileSetRegularDataRequest : BaseRequest<UserProfileSetRegularDataResponse>
    {
        public required UserProfileSetRegularDataCommand Command { get; init; }
    }
}
