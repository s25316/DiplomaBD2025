// Ignore Spelling: Dto
namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorization2Stage
{
    public class UserAuthorization2StageCommand
    {
        public required Guid UrlSegmentPart1 { get; init; }
        public required string UrlSegmentPart2 { get; init; }
        public required UserAuthorization2StageCodeDto CodeDto { get; init; }
    }
}
