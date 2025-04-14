using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Response
{
    public class OfferTemplateRemoveResponse : ResponseTemplate<ResponseCommandMetadata>
    {
        public static OfferTemplateRemoveResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            var intCode = (int)code;
            return new OfferTemplateRemoveResponse
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
