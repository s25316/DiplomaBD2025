// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanies.Request
{
    public class GetCompanyUserCompaniesRequest : RequestTemplate<GetCompanyUserCompaniesResponse>
    {
        // For single company
        public required Guid? CompanyId { get; init; }
        public required string? Regon { get; init; } = null;
        public required string? Nip { get; init; } = null;
        public required string? Krs { get; init; } = null;

        // Other Filters
        public required string? SearchText { get; init; } = null;

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required CompanyUserCompaniesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
