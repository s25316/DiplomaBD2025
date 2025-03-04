using UseCase.Roles.CompanyUser.Commands.OffersCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Request
{
    public class OffersCreateRequest : RequestTemplate<OffersCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required Guid OfferTemplateId { get; init; }
        public required IEnumerable<OfferCreateCommand> Commands { get; init; } = [];
    }
}
