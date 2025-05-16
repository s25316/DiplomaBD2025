using UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Response
{
    public class UserLoginInResult
    {
        public required bool IsNeed2Stage { get; init; }
        public required UserLoginIn2StageDto? User2StageData { get; init; }
        public required UserAuthorizationDataDto? AuthorizationData { get; init; }
    }
}
