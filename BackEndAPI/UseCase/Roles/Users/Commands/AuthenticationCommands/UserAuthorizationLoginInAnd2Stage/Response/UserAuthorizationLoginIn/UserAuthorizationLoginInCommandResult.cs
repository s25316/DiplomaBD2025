namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorizationLoginIn
{
    public class UserAuthorizationLoginInCommandResult
    {
        public required bool IsNeed2Stage { get; init; }
        public required UserAuthorizationLoginIn2StageDto? User2StageData { get; init; }
        public required UserLoginInDataDto? AuthorizationData { get; init; }
    }
}
