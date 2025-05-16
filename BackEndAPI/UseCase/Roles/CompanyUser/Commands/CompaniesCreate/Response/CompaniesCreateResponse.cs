using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Request;
using UseCase.Shared.Responses.CommandResults;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Response
{
    public class CompaniesCreateResponse :
        ItemResponse<IEnumerable<BaseCommandResult<CompanyCreateCommand>>>
    {
    }
}
