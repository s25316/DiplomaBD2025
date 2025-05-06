// Ignore Spelling: Dto
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.Response
{
    public class UserAuthorizationResponse : ResponseMetaData
    {
        // Property
        public required UserAuthorizationDataDto? Result { get; init; }


        // Methods
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
