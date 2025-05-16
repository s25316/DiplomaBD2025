using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferRemove.Response
{
    public class OfferRemoveResponse : ItemResponse<ResultMetadata>
    {
        public static OfferRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new OfferRemoveResponse
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
