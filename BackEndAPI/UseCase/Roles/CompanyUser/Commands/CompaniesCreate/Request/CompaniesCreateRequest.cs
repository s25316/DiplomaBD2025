using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Request
{
    public class CompaniesCreateRequest : RequestTemplate<CompaniesCreateResponse>
    {
        public required IEnumerable<CompanyCreateCommand> Commands { get; init; }
    }
}
