using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Response
{
    public class UserProfileActivateResponse : ResponseTemplate<ResponseCommandMetadata>
    {
        // Methods
        public static UserProfileActivateResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new UserProfileActivateResponse
            {
                HttpCode = code,
                Result = new ResponseCommandMetadata
                {
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? code.Description(),
                }
            };
        }
    }
}
