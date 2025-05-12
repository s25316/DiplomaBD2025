using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.Requests.BaseRequests
{
    public class GetOfferTemplatesRequest<IResponse> : BaseRequest<IResponse>
    {
        // Company Parameters
        public required Guid? CompanyId { get; init; }

        public required CompanyQueryParametersDto CompanyQueryParameters { get; init; }

        // Offer Template Parameters
        public required Guid? OfferTemplateId { get; init; }

        public required IEnumerable<int> SkillIds { get; init; } = [];

        // Shared Parameters
        public required string? SearchText { get; init; } = null;

        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required bool Ascending { get; init; }
    }
}
