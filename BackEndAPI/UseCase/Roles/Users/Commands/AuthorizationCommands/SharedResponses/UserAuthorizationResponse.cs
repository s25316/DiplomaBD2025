// Ignore Spelling: Dto
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses
{
    public class UserAuthorizationResponse : ItemResponse<UserAuthorizationDataDto>
    {
        public static UserAuthorizationResponse InvalidResponse()
        {
            return new UserAuthorizationResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Unauthorized,
                Result = null,
            };
        }

        public static UserAuthorizationResponse ValidResponse(
            UserAuthorizationDataDto item)
        {
            return new UserAuthorizationResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Ok,
                Result = item,
            };
        }
    }
}
