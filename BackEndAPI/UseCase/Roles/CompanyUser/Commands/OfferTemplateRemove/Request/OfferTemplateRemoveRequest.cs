using UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Request
{
    public class OfferTemplateRemoveRequest : RequestTemplate<OfferTemplateRemoveResponse>
    {
        public required Guid OfferTemplateId { get; init; }
    }
}
