using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserCreateOfferTemplates.Request
{
    public class CompanyUserCreateOfferTemplatesRequest : BaseRequest<CommandsResponse<CompanyUserCreateOfferTemplatesCommand>>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<CompanyUserCreateOfferTemplatesCommand> Commands { get; init; }
    }
}
