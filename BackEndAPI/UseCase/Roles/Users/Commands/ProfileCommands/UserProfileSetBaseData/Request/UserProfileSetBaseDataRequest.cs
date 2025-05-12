using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Request
{
    public class UserProfileSetBaseDataRequest : BaseRequest<UserProfileSetBaseDataResponse>
    {
        public required UserProfileSetBaseDataCommand Command { get; init; }
    }
}
