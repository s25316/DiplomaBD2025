using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates.Request
{
    public class CompanyUserGetOfferTemplatesRequest : GetOfferTemplatesRequest<ItemsResponse<CompanyUserOfferTemplateAndCompanyDto>>
    {
        // Other filters
        public required bool ShowRemoved { get; init; }

        // Sorting
        public required CompanyUserOfferTemplateOrderBy OrderBy { get; init; }
    }
}
