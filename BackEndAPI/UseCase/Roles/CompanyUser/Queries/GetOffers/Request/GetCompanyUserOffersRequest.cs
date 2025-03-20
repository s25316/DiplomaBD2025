// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Queries.GetOffers.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOffers.Response;
using UseCase.Shared.DTOs.Responses.Companies.Offers;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetOffers.Request
{
    public class GetCompanyUserOffersRequest : RequestTemplate<GetCompanyUserOffersResponse>
    {
        // For Single Offer
        public required Guid? OfferId { get; init; }

        // For Offers
        public required OfferStatus? Status { get; init; } // X

        // Company identification
        public required Guid? CompanyId { get; init; }
        public required string? Regon { get; init; } = null;
        public required string? Nip { get; init; } = null;
        public required string? Krs { get; init; } = null;


        // For Branches
        public required Guid? BranchId { get; init; }
        public required float? Lon { get; init; } = null;// X
        public required float? Lat { get; init; } = null;// X


        // For ContractCondition
        public required Guid? ContractConditionId { get; init; }
        public required bool? IsNegotiable { get; init; }
        public required bool? IsPaid { get; init; }
        public required decimal? SalaryPerHourMin { get; init; }
        public required decimal? SalaryPerHourMax { get; init; }
        public required decimal? SalaryMin { get; init; }
        public required decimal? SalaryMax { get; init; }
        public required int? HoursMin { get; init; }
        public required int? HoursMax { get; init; }
        public required IEnumerable<int> ParameterIds { get; init; } = [];


        // For OfferTemplate
        public required Guid? OfferTemplateId { get; init; }
        public required IEnumerable<int> SkillIds { get; init; } = [];


        // Other filters
        public required string? SearchText { get; init; } = null;

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required CompanyUserOffersOrderBy OrderBy { get; init; }// X
        public required bool Ascending { get; init; }// X
    }
}
