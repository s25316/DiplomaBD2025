using Domain.Shared.Enums;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetRegularData.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetRegularData.Response
{
    public class UserProfileSetRegularDataResponse : ResponseTemplate<ResponseCommandTemplate<UserProfileSetRegularDataCommand>>
    {
        // Methods
        public static UserProfileSetRegularDataResponse PrepareResponse(
            HttpCode code,
            UserProfileSetRegularDataCommand command,
            string? message = null)
        {
            var intCode = (int)code;
            return new UserProfileSetRegularDataResponse
            {
                HttpCode = code,
                Result = new ResponseCommandTemplate<UserProfileSetRegularDataCommand>
                {
                    Item = command,
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
