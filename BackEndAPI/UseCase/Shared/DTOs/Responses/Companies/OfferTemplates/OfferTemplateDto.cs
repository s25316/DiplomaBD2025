// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.Responses.Companies.OfferTemplates
{
    public class OfferTemplateDto
    {
        public Guid OfferTemplateId { get; set; }

        public Guid CompanyId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime? Removed { get; set; }

        public IEnumerable<OfferSkillDto> Skills { get; init; } = [];
    }
}
