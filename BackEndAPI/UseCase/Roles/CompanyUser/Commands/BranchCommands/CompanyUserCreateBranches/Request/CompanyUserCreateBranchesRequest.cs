using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserCreateBranches.Request
{
    public class CompanyUserCreateBranchesRequest : BaseRequest<CommandsResponse<CompanyUserCreateBranchesCommand>>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<CompanyUserCreateBranchesCommand> Commands { get; init; }
    }
}
