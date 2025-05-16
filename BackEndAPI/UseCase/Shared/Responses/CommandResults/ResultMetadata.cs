// Ignore Spelling: Metadata

namespace UseCase.Shared.Responses.CommandResults
{
    public class ResultMetadata
    {
        public required bool IsCorrect { get; init; }
        public required string Message { get; init; } = null!;
    }
}
