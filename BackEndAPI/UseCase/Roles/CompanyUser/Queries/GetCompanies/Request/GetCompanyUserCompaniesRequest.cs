// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanies.Request
{
    public class GetCompanyUserCompaniesRequest : RequestTemplate<GetCompanyUserCompaniesResponse>
    {
        // For single company
        public Guid? CompanyId { get; init; }
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;

        // Other Filters
        public string? SearchText { get; init; } = null;

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required CompanyUserCompaniesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
