using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.ContractConditionUpdate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionUpdate.Response
{
    public class ContractConditionUpdateResponse : ResponseTemplate<ResponseCommandTemplate<ContractConditionsUpdateCommand>>
    {
        public static ContractConditionUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            ContractConditionsUpdateCommand command)
        {
            return new ContractConditionUpdateResponse
            {
                HttpCode = code,
                Result = new ResponseCommandTemplate<ContractConditionsUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
