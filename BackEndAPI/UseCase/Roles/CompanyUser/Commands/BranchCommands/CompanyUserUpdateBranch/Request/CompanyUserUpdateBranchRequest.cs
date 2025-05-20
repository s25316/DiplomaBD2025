using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserUpdateBranch.Request
{
    public class CompanyUserUpdateBranchRequest : BaseRequest<CommandResponse<CompanyUserUpdateBranchCommand>>
    {
        public required Guid BranchId { get; init; }
        public required CompanyUserUpdateBranchCommand Command { get; init; }
    }
}
