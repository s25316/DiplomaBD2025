// Ignore Spelling: Dto
using Domain.Shared.Enums;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorizationLoginIn
{
    public class UserAuthorizationLoginInResponse : ResponseTemplate<UserAuthorizationLoginInCommandResult>
    {
        // Methods
        public static UserAuthorizationLoginInResponse InvalidResponse()
        {
            return new UserAuthorizationLoginInResponse
            {
                HttpCode = HttpCode.Unauthorized,
                Result = new UserAuthorizationLoginInCommandResult
                {
                    IsNeed2Stage = false,
                    User2StageData = null,
                    AuthorizationData = null,
                },
            };
        }

        public static UserAuthorizationLoginInResponse IsNeed2StageResponse(UserAuthorizationLoginIn2StageDto dto)
        {
            return new UserAuthorizationLoginInResponse
            {
                HttpCode = HttpCode.Ok,
                Result = new UserAuthorizationLoginInCommandResult
                {
                    IsNeed2Stage = true,
                    User2StageData = dto,
                    AuthorizationData = null,
                },
            };
        }

        public static UserAuthorizationLoginInResponse AuthorizationResponse(UserLoginInDataDto dto)
        {
            return new UserAuthorizationLoginInResponse
            {
                HttpCode = HttpCode.Ok,
                Result = new UserAuthorizationLoginInCommandResult
                {
                    IsNeed2Stage = false,
                    User2StageData = null,
                    AuthorizationData = dto,
                },
            };
        }
    }
}
