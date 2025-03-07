using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response
{
    public class OfferTemplatesCreateResponse : ResponseMetaData
    {
        public IEnumerable<ResponseCommandTemplate<OfferTemplateCommand>> Commands { get; init; } = [];

    }
}
