using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Response
{
    public class OffersCreateResponse : ResponseTemplate
    {
        public IEnumerable<ResponseItemTemplate<OfferCreateCommand>> Commands { get; init; } = [];
    }
}
