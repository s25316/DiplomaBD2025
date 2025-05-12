using UseCase.Roles.Guests.Queries.GuestGetContractConditions.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Guests.Queries.GuestGetContractConditions.Request
{
    public class GuestGetContractConditionsRequest : GetContractConditionsRequest<ItemsResponse<GuestContractConditionAndCompanyDto>>
    {
        // Sorting
        public required GuestContractConditionOrderBy OrderBy { get; init; }
    }
}
