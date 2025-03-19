using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.BranchesCreate.Response
{
    public class BranchesCreateResponse :
        ResponseTemplate<IEnumerable<ResponseCommandTemplate<BranchCreateCommand>>>
    {
    }
}
