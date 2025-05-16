using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonLogin.Request
{
    public class UserUpdatePersonLoginRequest : BaseRequest<ResultMetadataResponse>
    {
        public required UserUpdatePersonLoginCommand Command { get; init; }
    }
}
