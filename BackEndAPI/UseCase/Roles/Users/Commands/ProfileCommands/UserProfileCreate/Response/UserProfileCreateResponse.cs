using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate.Response
{
    public class UserProfileCreateResponse : ResponseTemplate<ResponseCommandMetadata>
    {
        // Methods
        public static UserProfileCreateResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new UserProfileCreateResponse
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
