// Ignore Spelling: Regon, Nip, Krs
using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Response;
using UseCase.Shared.Enums;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Request
{
    public class GetCompanyOfferTemplatesRequest : RequestTemplate<GetCompanyOfferTemplatesResponse>
    {
        // For selection single Template
        public Guid? OfferTemplateId { get; init; }

        // Company Identification  
        public Guid? CompanyId { get; init; }
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;

        // Other filters
        public string? SearchText { get; init; } = null;
        public IEnumerable<int> SkillIds { get; init; } = [];
        public required bool ShowRemoved { get; init; }

        // Pagination
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }

        // Sorting
        public required OfferTemplatesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
