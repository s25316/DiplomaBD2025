using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserUpdateOfferTemplate.Request
{
    public class CompanyUserUpdateOfferTemplateRequest : BaseRequest<CommandResponse<CompanyUserUpdateOfferTemplateCommand>>
    {
        public required Guid OfferTemplateId { get; init; }
        public required CompanyUserUpdateOfferTemplateCommand Command { get; init; }
    }
}
