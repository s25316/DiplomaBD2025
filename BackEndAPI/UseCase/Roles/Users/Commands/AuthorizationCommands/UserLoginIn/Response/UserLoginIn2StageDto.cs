// Ignore Spelling: Dto
namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Response
{
    public class UserLoginIn2StageDto
    {
        public required Guid UrlSegmentPart1 { get; init; }
        public required string UrlSegmentPart2 { get; init; }
        public required DateTime ValidTo { get; init; }
    }
}
