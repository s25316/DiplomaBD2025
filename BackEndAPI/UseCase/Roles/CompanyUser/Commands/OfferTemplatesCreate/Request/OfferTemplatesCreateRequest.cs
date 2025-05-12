using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request
{
    public class OfferTemplatesCreateRequest : BaseRequest<OfferTemplatesCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<OfferTemplateCreateCommand> Commands { get; init; }
    }
}
