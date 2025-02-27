using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response
{
    public class OfferTemplatesCreateResponse
    {
        public IEnumerable<BaseResponseGeneric<OfferTemplateCommand>> Commands { get; init; } = [];

    }
}
