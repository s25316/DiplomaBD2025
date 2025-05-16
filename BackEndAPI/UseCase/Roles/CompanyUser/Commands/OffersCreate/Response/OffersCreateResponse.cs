using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Response
{
    public class OffersCreateResponse :
        ItemResponse<IEnumerable<BaseCommandResult<OfferCreateCommand>>>
    {
    }
}
