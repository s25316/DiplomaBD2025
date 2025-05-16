using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserInitiateUpdatePersonPassword.Request
{
    public class UserInitiateUpdatePersonPasswordRequest : BaseRequest<ResultMetadataResponse>
    {
        public required UserInitiateUpdatePersonPasswordCommand Command { get; init; }
    }
}
