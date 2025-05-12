using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Guests.Queries.GuestGetCompanies.Request
{
    public class GuestGetCompaniesRequest : GetCompaniesRequest<ItemsResponse<CompanyDto>>
    {
    }
}
