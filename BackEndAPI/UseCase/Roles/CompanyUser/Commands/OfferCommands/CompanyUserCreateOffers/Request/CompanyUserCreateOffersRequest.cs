using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserCreateOffers.Request
{
    public class CompanyUserCreateOffersRequest : BaseRequest<CommandsResponse<CompanyUserCreateOffersCommand>>
    {
        public IEnumerable<CompanyUserCreateOffersCommand> Commands { get; init; } = [];
    }
}
