using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Response
{
    public class ContractConditionRemoveResponse : ResponseTemplate<ResponseCommandMetadata>
    {
        public static ContractConditionRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new ContractConditionRemoveResponse
            {
                HttpCode = code,
                Result = new ResponseCommandMetadata
                {
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? code.Description()
                }
            };
        }
    }
}
