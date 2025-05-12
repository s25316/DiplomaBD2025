// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches.Response;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.Requests;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Roles.CompanyUser.Queries.GetBranches.Request
{
    public class GetCompanyUserBranchesRequest
        : BaseRequest<GetCompanyUserGenericItemsResponse<CompanyUserGetBranchAndCompanyDto>>
    {
        // For single Company
        public required Guid? BranchId { get; init; }

        // Company identification
        public required Guid? CompanyId { get; init; }
        public required CompanyQueryParametersDto CompanyParameters { get; init; }

        // Other filters
        public required string? SearchText { get; init; } = null;
        public required bool ShowRemoved { get; init; }
        public required GeographyPointQueryParametersDto GeographyPoint { get; init; }

        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required CompanyUserBranchOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
