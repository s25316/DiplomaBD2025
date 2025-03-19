using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response
{
    public class OfferTemplatesCreateResponse
        : ResponseTemplate<IEnumerable<ResponseCommandTemplate<OfferTemplateCreateCommand>>>
    {
    }
}
