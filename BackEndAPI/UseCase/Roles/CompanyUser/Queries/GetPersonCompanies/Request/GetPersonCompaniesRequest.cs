// Ignore Spelling: Regon, Nip, Krs
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Response;
using UseCase.Shared.Enums;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Request
{
    public class GetPersonCompaniesRequest : RequestTemplate<GetPersonCompaniesResponse>
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
        public required CompaniesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
