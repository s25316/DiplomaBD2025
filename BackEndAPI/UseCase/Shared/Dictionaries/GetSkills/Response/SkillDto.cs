// Ignore Spelling: Dto

using UseCase.Shared.Dictionaries.GetSkillTypes.Response;

namespace UseCase.Shared.Dictionaries.GetSkills.Response
{
    public class SkillDto
    {
        public int SkillId { get; init; }

        public string Name { get; init; } = null!;

        public SkillTypeDto SkillType { get; init; } = null!;
    }
}
