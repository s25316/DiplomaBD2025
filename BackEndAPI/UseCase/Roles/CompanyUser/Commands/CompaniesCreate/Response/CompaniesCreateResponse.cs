using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Request;
using UseCase.Shared.Templates.Response.Commands;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Response
{
    public class CompaniesCreateResponse :
        ResponseTemplate<IEnumerable<ResponseCommandTemplate<CompanyCreateCommand>>>
    {
    }
}
