using UseCase.Roles.CompanyUser.Commands.BranchCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchCreate.Request
{
    public class BranchCreateRequest : BaseRequest<BranchCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<BranchCreateCommand> Commands { get; init; }
    }
}
