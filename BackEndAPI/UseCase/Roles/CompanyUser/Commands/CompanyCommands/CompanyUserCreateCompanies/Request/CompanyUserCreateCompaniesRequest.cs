using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserCreateCompanies.Request
{
    public class CompanyUserCreateCompaniesRequest : BaseRequest<CommandsResponse<CompanyUserCreateCompaniesCommand>>
    {
        public required IEnumerable<CompanyUserCreateCompaniesCommand> Commands { get; init; }
    }
}
