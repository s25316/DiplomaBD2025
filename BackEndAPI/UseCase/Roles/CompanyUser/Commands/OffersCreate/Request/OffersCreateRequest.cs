using UseCase.Roles.CompanyUser.Commands.OffersCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Request
{
    public class OffersCreateRequest : RequestTemplate<OffersCreateResponse>
    {
        public IEnumerable<OfferCreateCommand> Commands { get; init; } = [];
    }
}
