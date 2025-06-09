using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.Requests.BaseRequests
{
    public class GetPeopleRequest<IResponse> : BaseRequest<IResponse>
    {
        public required Guid? PersonId { get; init; }

        public required string? Email { get; init; }

        public required string? SearchText { get; init; }

        public required bool? ShowRemoved { get; init; }

        public required PaginationQueryParametersDto PaginationQueryParameters { get; init; }

        public required bool Ascending { get; init; }
    }
}
