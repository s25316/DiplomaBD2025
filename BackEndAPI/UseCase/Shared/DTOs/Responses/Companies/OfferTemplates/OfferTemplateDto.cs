// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.Responses.Companies.OfferTemplates
{
    public class OfferTemplateDto
    {
        public Guid OfferTemplateId { get; init; }
        public Guid CompanyId { get; init; }
        public string Name { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime Created { get; init; }
        public DateTime? Removed { get; init; }
        public IEnumerable<OfferSkillDto> Skills { get; init; } = [];
    }
}
