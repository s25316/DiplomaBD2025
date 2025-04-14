using UseCase.Roles.CompanyUser.Commands.BranchRemove.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchRemove.Request
{
    public class BranchRemoveRequest : RequestTemplate<BranchRemoveResponse>
    {
        public required Guid BranchId { get; init; }
    }
}
