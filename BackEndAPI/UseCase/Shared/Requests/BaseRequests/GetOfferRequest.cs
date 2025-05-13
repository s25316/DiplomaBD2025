using UseCase.Shared.Enums;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.Requests.BaseRequests
{
    public class GetOfferRequest<IResponse> : BaseRequest<IResponse>
    {
        // Company Parameters
        public required Guid? CompanyId { get; init; }

        public required CompanyQueryParametersDto CompanyQueryParameters { get; init; }

        // Offer Parameters
        public required Guid? OfferId { get; init; }

        public required OfferQueryParametersDto OfferQueryParameters { get; init; }

        // Branch Parameters
        public required Guid? BranchId { get; init; }

        public required GeographyPointQueryParametersDto GeographyPoint { get; init; } // Order By

        // ContractCondition Parameters
        public required Guid? ContractConditionId { get; init; }

        public required SalaryQueryParametersDto SalaryParameters { get; init; }

        public required IEnumerable<int> ContractParameterIds { get; init; } = [];

        // OfferTemplate Parameters
        public required Guid? OfferTemplateId { get; init; }

        public required IEnumerable<int> SkillIds { get; init; } = [];

        // Other Parameters
        public required string? SearchText { get; init; } = null;


        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required OfferOrderBy OrderBy { get; init; }

        public required bool Ascending { get; init; }
    }
}
