using UseCase.Roles.CompanyUser.Commands.OffersCreate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Request
{
    public class OffersCreateRequest : BaseRequest<OffersCreateResponse>
    {
        public IEnumerable<OfferCreateCommand> Commands { get; init; } = [];
    }
}
