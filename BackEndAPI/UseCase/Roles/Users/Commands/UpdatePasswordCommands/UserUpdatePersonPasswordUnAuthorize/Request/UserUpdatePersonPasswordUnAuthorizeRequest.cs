using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordUnAuthorize.Request
{
    public class UserUpdatePersonPasswordUnAuthorizeRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid UrlSegment1 { get; init; }
        public required string UrlSegment2 { get; init; }
        public required UserUpdatePersonPasswordUnAuthorizeCommand Command { get; init; }
    }
}
