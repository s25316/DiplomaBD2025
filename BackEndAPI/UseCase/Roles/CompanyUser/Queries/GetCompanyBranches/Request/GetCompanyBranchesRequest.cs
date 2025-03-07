// Ignore Spelling: Regon, Nip, Krs
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response;
using UseCase.Shared.Enums;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Request
{
    public class GetCompanyBranchesRequest : RequestTemplate<GetCompanyBranchesResponse>
    {
        public Guid? CompanyId { get; init; }
        public Guid? BranchId { get; init; }
        public string? SearchText { get; init; } = null;
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;
        public float? Lon { get; init; } = null;
        public float? Lat { get; init; } = null;
        public required bool ShowRemoved { get; init; }
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }
        public required BranchesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
