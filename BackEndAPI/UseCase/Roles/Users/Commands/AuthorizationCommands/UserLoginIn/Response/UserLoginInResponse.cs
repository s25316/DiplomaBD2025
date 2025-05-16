// Ignore Spelling: Dto
using Domain.Shared.Enums;
using UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Response
{
    public class UserLoginInResponse : ItemResponse<UserLoginInResult>
    {
        // Methods
        public static UserLoginInResponse InvalidResponse()
        {
            return new UserLoginInResponse
            {
                HttpCode = HttpCode.Unauthorized,
                Result = new UserLoginInResult
                {
                    IsNeed2Stage = false,
                    User2StageData = null,
                    AuthorizationData = null,
                },
            };
        }

        public static UserLoginInResponse IsNeed2StageResponse(UserLoginIn2StageDto dto)
        {
            return new UserLoginInResponse
            {
                HttpCode = HttpCode.Ok,
                Result = new UserLoginInResult
                {
                    IsNeed2Stage = true,
                    User2StageData = dto,
                    AuthorizationData = null,
                },
            };
        }

        public static UserLoginInResponse AuthorizationResponse(UserAuthorizationDataDto dto)
        {
            return new UserLoginInResponse
            {
                HttpCode = HttpCode.Ok,
                Result = new UserLoginInResult
                {
                    IsNeed2Stage = false,
                    User2StageData = null,
                    AuthorizationData = dto,
                },
            };
        }
    }
}
