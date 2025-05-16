using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.BranchesCreate.Response
{
    public class BranchesCreateResponse :
        ItemResponse<IEnumerable<BaseCommandResult<BranchCreateCommand>>>
    {
    }
}
