using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.Requests.BaseRequests
{
    public class GetBranchesRequest<TResponse> : BaseRequest<TResponse>
    {
        // Company Parameters
        public required Guid? CompanyId { get; init; }

        public required CompanyQueryParametersDto CompanyQueryParameters { get; init; }


        // Branch Parameters
        public required Guid? BranchId { get; init; }


        // Other Parameters
        public required string? SearchText { get; init; } = null;
        public required GeographyPointQueryParametersDto GeographyPoint { get; init; }

        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required bool Ascending { get; init; }
    }
}
