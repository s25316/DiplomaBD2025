using UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Request
{
    public class OfferTemplateUpdateRequest : RequestTemplate<OfferTemplateUpdateResponse>
    {
        public required Guid OfferTemplateId { get; init; }
        public required OfferTemplateUpdateCommand Command { get; init; }
    }
}
