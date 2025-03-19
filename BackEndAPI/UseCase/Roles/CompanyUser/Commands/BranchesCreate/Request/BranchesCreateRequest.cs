using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchesCreate.Request
{
    public class BranchesCreateRequest : RequestTemplate<BranchesCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<BranchCreateCommand> Commands { get; init; }
    }
}
