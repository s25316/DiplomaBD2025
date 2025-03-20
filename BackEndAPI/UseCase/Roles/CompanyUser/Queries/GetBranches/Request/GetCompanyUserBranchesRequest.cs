// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Queries.GetBranches.Enums;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetBranches.Request
{
    public class GetCompanyUserBranchesRequest : RequestTemplate<GetCompanyUserBranchesResponse>
    {
        // For single Company
        public required Guid? BranchId { get; init; }

        // Company identification
        public required Guid? CompanyId { get; init; }
        public required string? Regon { get; init; } = null;
        public required string? Nip { get; init; } = null;
        public required string? Krs { get; init; } = null;

        // Other filters
        public required string? SearchText { get; init; } = null;
        public required float? Lon { get; init; } = null;
        public required float? Lat { get; init; } = null;
        public required bool ShowRemoved { get; init; }

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required CompanyUserBranchesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
