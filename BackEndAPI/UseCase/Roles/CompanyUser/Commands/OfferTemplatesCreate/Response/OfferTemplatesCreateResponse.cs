using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response
{
    public class OfferTemplatesCreateResponse : ResponseTemplate
    {
        public IEnumerable<ResponseItemTemplate<OfferTemplateCommand>> Commands { get; init; } = [];

    }
}
