using UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Response
{
    public class ContractConditionsCreateResponse :
        ItemResponse<IEnumerable<BaseCommandResult<ContractConditionsCreateCommand>>>
    {
    }
}
