using UseCase.Roles.CompanyUser.Commands.OfferUpdate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferUpdate.Request
{
    public class OfferUpdateRequest : RequestTemplate<OfferUpdateResponse>
    {
        public required Guid OfferId { get; init; }
        public required OfferUpdateCommand Command { get; init; }
    }
}
