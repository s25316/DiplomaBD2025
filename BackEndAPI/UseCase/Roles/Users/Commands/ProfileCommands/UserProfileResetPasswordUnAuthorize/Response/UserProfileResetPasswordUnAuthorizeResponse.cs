using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordUnAuthorize.Response
{
    public class UserProfileResetPasswordUnAuthorizeResponse : ResponseMetaData
    {
        // Methods
        public static UserProfileResetPasswordUnAuthorizeResponse PrepareValid()
        {
            return new UserProfileResetPasswordUnAuthorizeResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Ok
            };
        }

        public static UserProfileResetPasswordUnAuthorizeResponse PrepareInvalid()
        {
            return new UserProfileResetPasswordUnAuthorizeResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.BadRequest
            };
        }
    }
}
