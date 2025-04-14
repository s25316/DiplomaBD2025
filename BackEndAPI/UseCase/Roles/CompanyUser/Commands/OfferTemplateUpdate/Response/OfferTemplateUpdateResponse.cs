using Domain.Shared.Enums;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Response
{
    public class OfferTemplateUpdateResponse
         : ResponseTemplate<ResponseCommandTemplate<OfferTemplateUpdateCommand>>
    {
        public static OfferTemplateUpdateResponse PrepareResponse(
            HttpCode code,
            string? message,
            OfferTemplateUpdateCommand command)
        {
            return new OfferTemplateUpdateResponse
            {
                HttpCode = code,
                Result = new ResponseCommandTemplate<OfferTemplateUpdateCommand>
                {
                    Item = command,
                    IsCorrect = (int)code < 300 && (int)code >= 200,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
