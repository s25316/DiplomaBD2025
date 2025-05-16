using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Response
{
    public class CompanyUpdateResponse
        : ItemResponse<BaseCommandResult<CompanyUpdateCommand>>
    {
        public static CompanyUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            CompanyUpdateCommand command)
        {
            return new CompanyUpdateResponse
            {
                HttpCode = code,
                Result = new BaseCommandResult<CompanyUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
