using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Response
{
    public class OffersCreateResponse
    {
        public IEnumerable<BaseResponseGeneric<OfferCreateCommand>> Commands { get; init; } = [];
    }
}
