using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserSetBasePersonData.Request
{
    public class UserSetBasePersonDataRequest : BaseRequest<CommandResponse<UserSetBasePersonDataCommand>>
    {
        public required UserSetBasePersonDataCommand Command { get; init; }
    }
}
