using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserRemoveOffer.Request
{
    public class CompanyUserRemoveOfferRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid OfferId { get; init; }
    }
}
