using UseCase.Shared.Templates.Response.QueryResults;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Queries.GetContractConditions.Response
{
    public class GetCompanyUserContractConditionsResponse
        : ResponseTemplate<ResponseQueryResultTemplate<CompanyAndContractConditionDto>>
    {
    }
}
