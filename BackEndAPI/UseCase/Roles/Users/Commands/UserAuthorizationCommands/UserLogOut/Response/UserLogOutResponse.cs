using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserLogOut.Response
{
    public class UserLogOutResponse : ResponseMetaData
    {
        // Methods
        public static UserLogOutResponse PrepareResponse(HttpCode code)
        {
            return new UserLogOutResponse
            {
                HttpCode = code,
            };
        }
    }
}
