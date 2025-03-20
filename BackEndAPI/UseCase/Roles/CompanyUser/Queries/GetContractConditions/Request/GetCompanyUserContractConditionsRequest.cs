// Ignore Spelling: Regon, Krs

using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetContractConditions.Request
{
    public class GetCompanyUserContractConditionsRequest : RequestTemplate<GetCompanyUserContractConditionsResponse>
    {
        // For single ContractCondition
        public required Guid? ContractConditionId { get; init; }

        // Company identification
        public required Guid? CompanyId { get; init; }
        public required string? Regon { get; init; } = null;
        public required string? Nip { get; init; } = null;
        public required string? Krs { get; init; } = null;

        // Other filters
        public required string? SearchText { get; init; }
        public required bool ShowRemoved { get; init; }
        public required bool? IsNegotiable { get; init; }
        public required bool? IsPaid { get; init; }
        public required decimal? SalaryPerHourMin { get; init; }
        public required decimal? SalaryPerHourMax { get; init; }
        public required decimal? SalaryMin { get; init; }
        public required decimal? SalaryMax { get; init; }
        public required int? HoursMin { get; init; }
        public required int? HoursMax { get; init; }
        public required IEnumerable<int> ParameterIds { get; init; } = [];

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required CompanyUserContractConditionsOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
