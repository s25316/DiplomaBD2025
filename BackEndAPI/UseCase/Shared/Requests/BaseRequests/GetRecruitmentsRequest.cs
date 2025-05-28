using Domain.Features.Recruitments.Enums;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.Requests.BaseRequests
{
    public class GetRecruitmentsRequest<TResponse> : BaseRequest<TResponse>
    {
        // Company Parameters
        public required CompanyQueryParametersDto CompanyQueryParameters { get; init; }

        // Offer Parameters
        public required OfferQueryParametersDto OfferQueryParameters { get; init; }

        // Branch Parameters
        //public required GeographyPointQueryParametersDto GeographyPoint { get; init; } // Order By

        // ContractCondition Parameters
        public required SalaryQueryParametersDto SalaryParameters { get; init; }

        public required IEnumerable<int> ContractParameterIds { get; init; } = [];

        // OfferTemplate Parameters
        public required IEnumerable<int> SkillIds { get; init; } = [];

        // Recruitment Parameters
        public required Guid? RecruitmentId { get; init; }
        public required ProcessType? ProcessType { get; init; }

        // Other Parameters
        public required string? SearchText { get; init; } = null;


        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        //public required OfferOrderBy OrderBy { get; init; }

        public required bool Ascending { get; init; }
    }
}
