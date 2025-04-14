using UseCase.Roles.CompanyUser.Commands.BranchUpdate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchUpdate.Request
{
    public class BranchUpdateRequest : RequestTemplate<BranchUpdateResponse>
    {
        public required Guid BranchId { get; init; }
        public required BranchUpdateCommand Command { get; init; }
    }
}
