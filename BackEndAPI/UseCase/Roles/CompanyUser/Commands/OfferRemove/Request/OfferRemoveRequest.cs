using UseCase.Roles.CompanyUser.Commands.OfferRemove.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferRemove.Request
{
    public class OfferRemoveRequest : BaseRequest<OfferRemoveResponse>
    {
        public required Guid OfferId { get; init; }
    }
}
