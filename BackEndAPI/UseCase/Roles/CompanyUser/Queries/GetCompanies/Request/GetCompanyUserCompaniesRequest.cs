// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.DTOs.QueryParameters;
using UseCase.Shared.DTOs.Responses.Companies;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanies.Request
{
    public class GetCompanyUserCompaniesRequest :
        RequestTemplate<GetCompanyUserGenericItemsResponse<CompanyDto>>
    {
        // For single company
        public required Guid? CompanyId { get; init; }
        public required CompanyQueryParametersDto CompanyParameters { get; init; }

        // Other Filters
        public required string? SearchText { get; init; } = null;

        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required CompanyUserCompaniesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
