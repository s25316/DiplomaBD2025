using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Response
{
    public class OffersCreateResponse : ResponseMetaData
    {
        public IEnumerable<ResponseCommandTemplate<OfferCreateCommand>> Commands { get; init; } = [];
    }
}
