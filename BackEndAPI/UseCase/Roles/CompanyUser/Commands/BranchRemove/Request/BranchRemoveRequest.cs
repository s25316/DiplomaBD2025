using UseCase.Roles.CompanyUser.Commands.BranchRemove.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.BranchRemove.Request
{
    public class BranchRemoveRequest : BaseRequest<BranchRemoveResponse>
    {
        public required Guid BranchId { get; init; }
    }
}
