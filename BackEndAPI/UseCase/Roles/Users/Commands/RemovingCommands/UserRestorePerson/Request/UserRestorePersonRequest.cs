using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.RemovingCommands.UserRestorePerson.Request
{
    public class UserRestorePersonRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid UrlSegment1 { get; init; }
        public required string UrlSegment2 { get; init; }
    }
}
