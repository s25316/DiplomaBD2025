// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.Responses
{
    public class SkillResponseDto
    {
        public required int SkillId { get; init; }
        public required string Name { get; init; } = null!;
        public required string Description { get; init; } = null!;
        public required SkillTypeDto SkillType { get; init; }
    }
}
