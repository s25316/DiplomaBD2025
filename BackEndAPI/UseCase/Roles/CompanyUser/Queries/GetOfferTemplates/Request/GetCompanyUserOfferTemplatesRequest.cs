// Ignore Spelling: Regon, Krs

using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Request
{
    public class GetCompanyUserOfferTemplatesRequest : RequestTemplate<GetCompanyUserOfferTemplatesResponse>
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
        public required CompanyUserOfferTemplatesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
