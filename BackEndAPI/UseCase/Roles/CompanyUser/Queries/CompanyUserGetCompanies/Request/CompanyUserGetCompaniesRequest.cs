using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetCompanies.Request
{
    public class CompanyUserGetCompaniesRequest : GetCompaniesRequest<ItemsResponse<CompanyDto>>
    {
    }
}
