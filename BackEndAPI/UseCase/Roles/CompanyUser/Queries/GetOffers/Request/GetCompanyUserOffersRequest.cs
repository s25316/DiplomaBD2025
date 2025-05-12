// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Queries.GetOffers.Enums;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.DTOs.Responses.Companies.Offers;
using UseCase.Shared.Requests;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Roles.CompanyUser.Queries.GetOffers.Request
{
    public class GetCompanyUserOffersRequest :
        BaseRequest<GetCompanyUserGenericItemsResponse<OfferDto>>
    {
        // For Single Offer
        public required Guid? OfferId { get; init; }
        public required OfferQueryParametersDto OfferParameters { get; init; }

        // Company identification
        public required Guid? CompanyId { get; init; }
        public required CompanyQueryParametersDto CompanyParameters { get; init; }


        // For Branches
        public required Guid? BranchId { get; init; }
        public required GeographyPointQueryParametersDto GeographyPoint { get; init; }// X


        // For ContractCondition
        public required Guid? ContractConditionId { get; init; }
        public required SalaryQueryParametersDto SalaryParameters { get; init; }
        public required IEnumerable<int> ContractParameterIds { get; init; } = [];


        // For OfferTemplate
        public required Guid? OfferTemplateId { get; init; }
        public required IEnumerable<int> SkillIds { get; init; } = [];


        // Company Branch OfferTemplate
        public required string? SearchText { get; init; } = null;

        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required CompanyUserOffersOrderBy OrderBy { get; init; }// X
        public required bool Ascending { get; init; }// X
    }
}
