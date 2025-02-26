namespace UseCase.Shared.Templates.Response
{
    public class BaseResponse
    {
        public bool IsSuccess { get; init; }
        public required string Message { get; init; } = null!;
    }
}
