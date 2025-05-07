using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate.Response
{
    public class UserProfileResetPasswordInitiateResponse : ResponseMetaData
    {
        // Methods
        public static UserProfileResetPasswordInitiateResponse PrepareValid()
        {
            return new UserProfileResetPasswordInitiateResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.Ok
            };
        }

        public static UserProfileResetPasswordInitiateResponse PrepareInvalid()
        {
            return new UserProfileResetPasswordInitiateResponse
            {
                HttpCode = Domain.Shared.Enums.HttpCode.BadRequest
            };
        }
    }
}
