using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.OfferUpdate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.OfferUpdate.Response
{
    public class OfferUpdateResponse
        : ResponseTemplate<ResponseCommandTemplate<OfferUpdateCommand>>
    {
        public static OfferUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            OfferUpdateCommand command)
        {
            return new OfferUpdateResponse
            {
                HttpCode = code,
                Result = new ResponseCommandTemplate<OfferUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
