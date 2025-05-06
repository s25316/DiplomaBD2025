using UseCase.Roles.Users.Commands.UserAuthorizationCommands.Response;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginIn.Response
{
    public class UserAuthorizationLoginInCommandResult
    {
        public required bool IsNeed2Stage { get; init; }
        public required UserAuthorizationLoginIn2StageDto? User2StageData { get; init; }
        public required UserAuthorizationDataDto? AuthorizationData { get; init; }
    }
}
