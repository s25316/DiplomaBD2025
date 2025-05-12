using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.Requests.BaseRequests
{
    public class GetContractConditionsRequest<IResponse> : BaseRequest<IResponse>
    {
        // Company Parameters
        public required Guid? CompanyId { get; init; }

        public required CompanyQueryParametersDto CompanyQueryParameters { get; init; }


        // ContractCondition Parameters
        public required Guid? ContractConditionId { get; init; }

        public required IEnumerable<int> ContractParameterIds { get; init; } = [];

        public required SalaryQueryParametersDto SalaryParameters { get; init; }


        // Other Parameters
        public required string? SearchText { get; init; } = null;

        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required bool Ascending { get; init; }
    }
}
