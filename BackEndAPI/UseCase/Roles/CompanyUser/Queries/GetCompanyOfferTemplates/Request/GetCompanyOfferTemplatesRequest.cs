// Ignore Spelling: Regon, Nip, Krs
using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Response;
using UseCase.Shared.Enums;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Request
{
    public class GetCompanyOfferTemplatesRequest : RequestTemplate<GetCompanyOfferTemplatesResponse>
    {
        public Guid? CompanyId { get; init; }
        public Guid? OfferTemplateId { get; init; }
        public string? SearchText { get; init; } = null;
        public string? Regon { get; init; } = null;
        public string? Nip { get; init; } = null;
        public string? Krs { get; init; } = null;
        public IEnumerable<int> SkillIds { get; init; } = [];
        public required bool ShowRemoved { get; init; }
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }
        public required OfferTemplatesOrderBy OrderBy { get; init; }
        public required bool Ascending { get; init; }
    }
}
