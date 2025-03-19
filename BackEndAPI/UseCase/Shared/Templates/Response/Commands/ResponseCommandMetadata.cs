// Ignore Spelling: Metadata

namespace UseCase.Shared.Templates.Response.Commands
{
    public class ResponseCommandMetadata
    {
        public required bool IsCorrect { get; init; }
        public required string Message { get; init; } = null!;
    }
}
