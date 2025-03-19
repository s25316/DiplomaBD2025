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
        public Guid? OfferId { get; init; }

        // For Offers
        public OfferStatus? Status { get; init; } // X

        // Company identification
        public Guid? CompanyId { get; init; }
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;


        // For Branches
        public Guid? BranchId { get; init; }
        public float? Lon { get; init; } = null;// X
        public float? Lat { get; init; } = null;// X


        // For ContractCondition
        public Guid? ContractConditionId { get; init; }
        public bool? IsNegotiable { get; init; }
        public bool? IsPaid { get; init; }
        public decimal? SalaryPerHourMin { get; init; }
        public decimal? SalaryPerHourMax { get; init; }
        public decimal? SalaryMin { get; init; }
        public decimal? SalaryMax { get; init; }
        public int? HoursMin { get; init; }
        public int? HoursMax { get; init; }
        public IEnumerable<int> ParameterIds { get; init; } = [];


        // For OfferTemplate
        public Guid? OfferTemplateId { get; init; }
        public IEnumerable<int> SkillIds { get; init; } = [];


        // Other filters
        public string? SearchText { get; init; } = null;

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required CompanyUserOffersOrderBy OrderBy { get; init; }// X
        public required bool Ascending { get; init; }// X
    }
}
