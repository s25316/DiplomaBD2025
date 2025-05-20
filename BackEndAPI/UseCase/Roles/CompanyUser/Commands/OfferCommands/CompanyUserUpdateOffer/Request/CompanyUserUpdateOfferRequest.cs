using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserUpdateOffer.Request
{
    public class CompanyUserUpdateOfferRequest : BaseRequest<CommandResponse<CompanyUserUpdateOfferCommand>>
    {
        public required Guid OfferId { get; init; }
        public required CompanyUserUpdateOfferCommand Command { get; init; }
    }
}
