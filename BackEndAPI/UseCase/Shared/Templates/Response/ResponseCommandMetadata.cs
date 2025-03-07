namespace UseCase.Shared.Templates.Response
{
    public class ResponseCommandMetaData
    {
        public required bool IsCorrect { get; init; }
        public required string Message { get; init; } = null!;
    }
}
