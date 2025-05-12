using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchesCreate.Request
{
    public class BranchesCreateRequest : BaseRequest<BranchesCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<BranchCreateCommand> Commands { get; init; }
    }
}
