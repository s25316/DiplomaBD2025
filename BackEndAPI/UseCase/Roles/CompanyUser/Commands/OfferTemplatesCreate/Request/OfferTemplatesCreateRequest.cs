using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request
{
    public class OfferTemplatesCreateRequest : RequestTemplate<OfferTemplatesCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<OfferTemplateCreateCommand> Commands { get; init; }
    }
}
