// Ignore Spelling: Regon Krs

using UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Request
{
    public class GetCompanyOffersRequest : RequestTemplate<GetCompanyOffersResponse>
    {
        // For single Offer
        public Guid? OfferId { get; init; }

        // Company Identification
        public Guid? CompanyId { get; init; }
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;

        // Branch
        public Guid? BranchId { get; init; }
        public float? Lon { get; init; }
        public float? Lat { get; init; }

        // Offer Template
        public Guid? OfferTemplateId { get; init; }
        public IEnumerable<int> SkillIds { get; init; } = [];

        // Other Filters
        public string? SearchText { get; init; }
        public DateTime? PublicationStart { get; init; }
        public DateTime? PublicationEnd { get; init; }
        public DateTime? WorkBegin { get; init; }
        public DateTime? WorkEnd { get; init; }
        public float? MinSalary { get; init; }
        public float? MaxSalary { get; init; }
        public bool? IsNegotiated { get; init; }
        public int? CurrencyId { get; init; }
        public IEnumerable<int> SalaryTermIds { get; init; } = [];
        public IEnumerable<int> WorkModeIds { get; init; } = [];
        public IEnumerable<int> EmploymentTypeIds { get; init; } = [];

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required bool Ascending { get; init; }
    }
}
