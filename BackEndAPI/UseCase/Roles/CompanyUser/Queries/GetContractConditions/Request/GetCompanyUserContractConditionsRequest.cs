// Ignore Spelling: Regon, Krs

using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetContractConditions.Request
{
    public class GetCompanyUserContractConditionsRequest : RequestTemplate<GetCompanyUserContractConditionsResponse>
    {
        // For single ContractCondition
        public Guid? ContractConditionId { get; init; }

        // Company identification
        public Guid? CompanyId { get; init; }
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;

        // Other filters
        public string? SearchText { get; init; }
        public bool ShowRemoved { get; init; }
        public bool? IsNegotiable { get; init; }
        public bool? IsPaid { get; init; }
        public decimal? SalaryPerHourMin { get; init; }
        public decimal? SalaryPerHourMax { get; init; }
        public decimal? SalaryMin { get; init; }
        public decimal? SalaryMax { get; init; }
        public int? HoursMin { get; init; }
        public int? HoursMax { get; init; }
        public IEnumerable<int> ParameterIds { get; init; } = [];

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required CompanyUserContractConditionsOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
