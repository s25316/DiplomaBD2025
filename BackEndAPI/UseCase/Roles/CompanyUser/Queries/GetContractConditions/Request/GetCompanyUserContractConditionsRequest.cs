using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Response;
using UseCase.Shared.DTOs.QueryParameters;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetContractConditions.Request
{
    public class GetCompanyUserContractConditionsRequest : RequestTemplate<GetCompanyUserContractConditionsResponse>
    {
        // For single ContractCondition
        public required Guid? ContractConditionId { get; init; }

        // Company identification
        public required Guid? CompanyId { get; init; }
        public required CompanyQueryParametersDto CompanyParameters { get; init; }

        // Other filters
        public required string? SearchText { get; init; }
        public required bool ShowRemoved { get; init; }
        public required IEnumerable<int> ContractParameterIds { get; init; } = [];
        public required SalaryQueryParametersDto SalaryParameters { get; init; }

        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required CompanyUserContractConditionsOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
