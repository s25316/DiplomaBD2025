using UseCase.Roles.CompanyUser.Commands.OfferUpdate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferUpdate.Request
{
    public class OfferUpdateRequest : BaseRequest<OfferUpdateResponse>
    {
        public required Guid OfferId { get; init; }
        public required OfferUpdateCommand Command { get; init; }
    }
}
