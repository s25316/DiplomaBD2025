// Ignore Spelling: Dto
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorization2Stage
{
    public class UserAuthorization2StageResponse : ResponseMetaData
    {
        // Property
        public required UserLoginInDataDto? Result { get; init; }


        // Methods
        public static UserAuthorization2StageResponse InvalidResponse()
        {
            return new UserAuthorization2StageResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Unauthorized,
                Result = null,
            };
        }

        public static UserAuthorization2StageResponse ValidResponse(
            UserLoginInDataDto item)
        {
            return new UserAuthorization2StageResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Ok,
                Result = item,
            };
        }
    }
}
