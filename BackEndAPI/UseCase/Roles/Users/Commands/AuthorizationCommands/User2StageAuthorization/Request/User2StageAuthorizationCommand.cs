// Ignore Spelling: Dto
namespace UseCase.Roles.Users.Commands.AuthorizationCommands.User2StageAuthorization.Request
{
    public class User2StageAuthorizationCommand
    {
        public required Guid UrlSegmentPart1 { get; init; }
        public required string UrlSegmentPart2 { get; init; }
        public required User2StageCodeDto CodeDto { get; init; }
    }
}
