using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.OfferUpdate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferUpdate.Response
{
    public class OfferUpdateResponse
        : ItemResponse<BaseCommandResult<OfferUpdateCommand>>
    {
        public static OfferUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            OfferUpdateCommand command)
        {
            return new OfferUpdateResponse
            {
                HttpCode = code,
                Result = new BaseCommandResult<OfferUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
