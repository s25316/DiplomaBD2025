using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Response
{
    public class OfferTemplateRemoveResponse : ItemResponse<ResultMetadata>
    {
        public static OfferTemplateRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new OfferTemplateRemoveResponse
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
