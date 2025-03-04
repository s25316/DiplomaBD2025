namespace UseCase.Shared.Templates.Response
{
    public class ResponseItemMetaData
    {
        public required bool IsCorrect { get; init; }
        public required string Message { get; init; } = null!;
    }
}
