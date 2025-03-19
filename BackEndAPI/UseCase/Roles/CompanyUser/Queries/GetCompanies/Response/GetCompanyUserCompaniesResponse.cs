using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Templates.Response.QueryResults;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanies.Response
{
    public class GetCompanyUserCompaniesResponse
        : ResponseTemplate<ResponseQueryResultTemplate<CompanyDto>>
    {
    }
}
