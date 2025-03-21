// Ignore Spelling: Regon, Krs

using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Response;
using UseCase.Shared.DTOs.QueryParameters;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Request
{
    public class GetCompanyUserOfferTemplatesRequest : RequestTemplate<GetCompanyUserOfferTemplatesResponse>
    {
        // For selection single Template
        public required Guid? OfferTemplateId { get; init; }

        // Company Identification  
        public required Guid? CompanyId { get; init; }
        public required CompanyQueryParametersDto CompanyParameters { get; init; }

        // Other filters
        public required bool ShowRemoved { get; init; }
        public required string? SearchText { get; init; } = null;
        public required IEnumerable<int> SkillIds { get; init; } = [];

        // Pagination
        public required PaginationQueryParametersDto Pagination { get; init; }

        // Sorting
        public required CompanyUserOfferTemplatesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
