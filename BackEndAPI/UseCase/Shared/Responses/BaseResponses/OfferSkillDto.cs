﻿// Ignore Spelling: Dto
using UseCase.Shared.Dictionaries.GetSkills.Response;

namespace UseCase.Shared.Responses.BaseResponses
{
    public class OfferSkillDto
    {
        public bool IsRequired { get; init; }

        public SkillDto Skill { get; init; } = null!;
    }
}
