// Ignore Spelling: Regon, Nip, Krs
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response;
using UseCase.Shared.Enums;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Request
{
    public class GetCompanyBranchesRequest : RequestTemplate<GetCompanyBranchesResponse>
    {
        // For single Company
        public Guid? BranchId { get; init; }

        // Company identification
        public Guid? CompanyId { get; init; }
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;

        // Other filters
        public string? SearchText { get; init; } = null;
        public float? Lon { get; init; } = null;
        public float? Lat { get; init; } = null;
        public required bool ShowRemoved { get; init; }

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required BranchesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
