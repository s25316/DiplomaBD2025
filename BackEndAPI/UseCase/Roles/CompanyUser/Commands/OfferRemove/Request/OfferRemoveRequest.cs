using UseCase.Roles.CompanyUser.Commands.OfferRemove.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferRemove.Request
{
    public class OfferRemoveRequest : RequestTemplate<OfferRemoveResponse>
    {
        public required Guid OfferId { get; init; }
    }
}
