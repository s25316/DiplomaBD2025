// Ignore Spelling: Dto
using UseCase.Shared.Dictionaries.GetProcessTypes.Response;

namespace UseCase.Shared.Responses.BaseResponses
{
    public class RecruitmentDto
    {
        public Guid ProcessId { get; init; }

        public Guid OfferId { get; init; }

        public Guid PersonId { get; init; }

        public int ProcessTypeId { get; init; }

        public string? Message { get; init; }

        public DateTime Created { get; init; }

        public ProcessTypeDto ProcessType { get; init; } = null!;
    }
}
