using UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Response
{
    public class ContractConditionsCreateResponse :
        ResponseTemplate<IEnumerable<ResponseCommandTemplate<ContractConditionsCreateCommand>>>
    {
    }
}
