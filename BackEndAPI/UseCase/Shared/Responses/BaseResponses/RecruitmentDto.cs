// Ignore Spelling: Dto
namespace UseCase.Shared.Responses.BaseResponses
{
    public class RecruitmentDto
    {
        public Guid ProcessId { get; set; }

        public Guid OfferId { get; set; }

        public Guid PersonId { get; set; }

        public int ProcessTypeId { get; set; }

        public string? Message { get; set; }

        public string File { get; set; } = null!;

        public DateTime Created { get; set; }
    }
}
