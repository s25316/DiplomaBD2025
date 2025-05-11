using Domain.Shared.Enums;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Response
{
    public class UserProfileSetBaseDataResponse : ResponseTemplate<ResponseCommandTemplate<UserProfileSetBaseDataCommand>>
    {
        // Methods
        public static UserProfileSetBaseDataResponse PrepareResponse(
            HttpCode code,
            UserProfileSetBaseDataCommand command,
            string? message = null)
        {
            var intCode = (int)code;
            return new UserProfileSetBaseDataResponse
            {
                HttpCode = code,
                Result = new ResponseCommandTemplate<UserProfileSetBaseDataCommand>
                {
                    Item = command,
                    IsCorrect = intCode > 200 && intCode < 300,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
