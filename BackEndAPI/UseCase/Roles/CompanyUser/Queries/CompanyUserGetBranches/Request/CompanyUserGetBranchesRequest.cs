using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches.Request
{
    public class CompanyUserGetBranchesRequest : GetBranchesRequest<ItemsResponse<CompanyUserGetBranchAndCompanyDto>>
    {
        // Other Parameters
        public required bool ShowRemoved { get; init; }

        // Sorting
        public required CompanyUserBranchOrderBy OrderBy { get; init; }
    }
}
