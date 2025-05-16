using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Response
{
    public class ContractConditionRemoveResponse : ItemResponse<ResultMetadata>
    {
        public static ContractConditionRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new ContractConditionRemoveResponse
            {
                HttpCode = code,
                Result = new ResultMetadata
                {
                    IsCorrect = intCode >= 200 && intCode < 300,
                    Message = message ?? code.Description()
                }
            };
        }
    }
}
