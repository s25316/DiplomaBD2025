using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize.Response
{
    public class UserProfileResetPasswordAuthorizeResponse : ResponseMetaData
    {
        // Methods
        public static UserProfileResetPasswordAuthorizeResponse PrepareValid()
        {
            return new UserProfileResetPasswordAuthorizeResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Ok
            };
        }

        public static UserProfileResetPasswordAuthorizeResponse PrepareInvalid()
        {
            return new UserProfileResetPasswordAuthorizeResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.BadRequest
            };
        }
    }
}
