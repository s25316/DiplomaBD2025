using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Request
{
    public class CompaniesCreateRequest : BaseRequest<CompaniesCreateResponse>
    {
        public required IEnumerable<CompanyCreateCommand> Commands { get; init; }
    }
}
