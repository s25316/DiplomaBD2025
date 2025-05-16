using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.BranchRemove.Response
{
    public class BranchRemoveResponse : ItemResponse<ResultMetadata>
    {
        public static BranchRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new BranchRemoveResponse
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
