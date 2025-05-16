using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordAuthorize.Request
{
    public class UserUpdatePersonPasswordAuthorizeRequest : BaseRequest<ResultMetadataResponse>
    {
        public required UserUpdatePersonPasswordAuthorizeCommand Command { get; init; }
    }
}
