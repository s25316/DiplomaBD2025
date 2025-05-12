using UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Request
{
    public class OfferTemplateRemoveRequest : BaseRequest<OfferTemplateRemoveResponse>
    {
        public required Guid OfferTemplateId { get; init; }
    }
}
