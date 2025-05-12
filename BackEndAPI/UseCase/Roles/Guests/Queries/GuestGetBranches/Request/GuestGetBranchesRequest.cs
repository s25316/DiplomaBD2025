using UseCase.Roles.Guests.Queries.GuestGetBranches.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Guests.Queries.GuestGetBranches.Request
{
    public class GuestGetBranchesRequest : GetBranchesRequest<ItemsResponse<GuestBranchAndCompanyDto>>
    {
        // Sorting: Order By
        public required GuestBranchOrderBy OrderBy { get; init; }
    }
}
