// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.Responses.Dictionaries
{
    public class SkillTypeDto
    {
        public required int SkillTypeId { get; init; }
        public required string Name { get; init; } = null!;
        public required string Description { get; init; } = null!;
    }
}
