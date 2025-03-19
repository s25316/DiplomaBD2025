// Ignore Spelling: Dto

namespace UseCase.Shared.Dictionaries.GetFaqs.Response
{
    public class FaqDto
    {
        public Guid FaqId { get; init; }

        public string Question { get; init; } = null!;

        public string Answer { get; init; } = null!;

        public DateTime Created { get; init; }
    }
}
