using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonData.Request
{
    public class UserUpdatePersonDataRequest : BaseRequest<CommandResponse<UserUpdatePersonDataCommand>>
    {
        public required UserUpdatePersonDataCommand Command { get; init; }
    }
}
