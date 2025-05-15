using UseCase.Shared.Enums;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.Requests.BaseRequests
{
    public class GetCompaniesRequest<TResponse> : BaseRequest<TResponse>
    {
        // Parameters
        public required Guid? CompanyId { get; init; }

        public required CompanyQueryParametersDto CompanyQueryParameters { get; init; }

        public required string? SearchText { get; init; }

        //  Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required CompanyOrderBy OrderBy { get; init; }

        public required bool Ascending { get; init; }
    }
}
