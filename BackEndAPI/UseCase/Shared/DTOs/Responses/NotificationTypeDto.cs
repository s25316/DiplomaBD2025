// Ignore Spelling: Dto

namespace UseCase.Shared.DTOs.Responses
{
    public class NotificationTypeDto
    {
        public int NotificationTypeId { get; init; }
        public string Name { get; init; } = null!;
        public string Description { get; init; } = null!;
    }
}
