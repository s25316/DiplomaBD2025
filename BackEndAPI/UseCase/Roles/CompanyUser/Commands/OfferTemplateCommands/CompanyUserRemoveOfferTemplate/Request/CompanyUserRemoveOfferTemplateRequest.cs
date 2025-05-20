using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserRemoveOfferTemplate.Request
{
    public class CompanyUserRemoveOfferTemplateRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid OfferTemplateId { get; init; }
    }
}
