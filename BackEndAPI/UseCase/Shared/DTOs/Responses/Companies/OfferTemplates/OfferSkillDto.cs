// Ignore Spelling: Dto

using UseCase.Shared.Dictionaries.GetSkills.Response;

namespace UseCase.Shared.DTOs.Responses.Companies.OfferTemplates
{
    public class OfferSkillDto
    {
        public bool IsRequired { get; set; }

        public SkillDto Skill { get; init; } = null!;
    }
}
