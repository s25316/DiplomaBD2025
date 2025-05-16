using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Response
{
    public class OfferTemplatesCreateResponse
        : ItemResponse<IEnumerable<BaseCommandResult<OfferTemplateCreateCommand>>>
    {
    }
}
