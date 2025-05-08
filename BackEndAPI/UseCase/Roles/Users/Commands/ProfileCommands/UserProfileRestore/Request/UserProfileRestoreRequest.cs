using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileRestore.Request
{
    public class UserProfileRestoreRequest : RequestTemplate<ProfileCommandResponse>
    {
        public required Guid UrlSegment1 { get; init; }
        public required string UrlSegment2 { get; init; }
    }
}
