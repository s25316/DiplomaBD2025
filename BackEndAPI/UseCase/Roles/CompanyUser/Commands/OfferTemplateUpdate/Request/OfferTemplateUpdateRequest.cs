using UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Request
{
    public class OfferTemplateUpdateRequest : BaseRequest<OfferTemplateUpdateResponse>
    {
        public required Guid OfferTemplateId { get; init; }
        public required OfferTemplateUpdateCommand Command { get; init; }
    }
}
