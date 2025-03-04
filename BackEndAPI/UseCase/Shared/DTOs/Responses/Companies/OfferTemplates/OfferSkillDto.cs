// Ignore Spelling: Dto

using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Shared.DTOs.Responses.Companies.OfferTemplates
{
    public class OfferSkillDto
    {
        public SkillResponseDto Skill { get; init; } = null!;
        public bool IsRequired { get; init; }
    }
}
