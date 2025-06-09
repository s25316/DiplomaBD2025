using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetCompanies.Request
{
    public class AdministratorGetCompaniesRequest : GetCompaniesRequest<ItemsResponse<CompanyDto>>
    {
        public required bool? ShowRemoved { get; init; }
    }
}
