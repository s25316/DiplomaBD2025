using Domain.Features.Recruitments.Enums;
using UseCase.Roles.Users.Queries.GetPersonRecruitments.Response;
using UseCase.Shared.Requests;
using UseCase.Shared.Requests.QueryParameters;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Users.Queries.GetPersonRecruitments.Request
{
    public class GetPersonRecruitmentsRequest : BaseRequest<ItemsResponse<RecruitmentDataDto>>
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

        // Other Recruitment
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
