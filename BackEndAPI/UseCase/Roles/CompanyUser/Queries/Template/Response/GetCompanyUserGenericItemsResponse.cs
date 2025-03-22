// Ignore Spelling: Dto

using UseCase.Shared.Templates.Response.QueryResults;
using UseCase.Shared.Templates.Response.Responses;

namespace UseCase.Roles.CompanyUser.Queries.Template.Response
{
    public class GetCompanyUserGenericItemsResponse<TDto> :
        ResponseTemplate<ResponseQueryResultTemplate<TDto>>
    {
    }
}
