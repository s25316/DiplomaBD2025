using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.ContractConditionUpdate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionUpdate.Response
{
    public class ContractConditionUpdateResponse : ItemResponse<BaseCommandResult<ContractConditionsUpdateCommand>>
    {
        public static ContractConditionUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            ContractConditionsUpdateCommand command)
        {
            return new ContractConditionUpdateResponse
            {
                HttpCode = code,
                Result = new BaseCommandResult<ContractConditionsUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
