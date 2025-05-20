using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserRemoveBranch.Request
{
    public class CompanyUserRemoveBranchRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid BranchId { get; init; }
    }
}
