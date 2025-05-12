using UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions.Request
{
    public class CompanyUserGetContractConditionsRequest : GetContractConditionsRequest<ItemsResponse<CompanyUserContractConditionAndCompanyDto>>
    {
        // Contract Condition Parameters
        public required bool ShowRemoved { get; init; }


        // Sorting
        public required CompanyUserContractConditionOrderBy OrderBy { get; init; }
    }
}
