namespace UseCase.Shared.Responses.BaseResponses.Administrator
{
    public class ExceptionDto
    {
        public Guid ExceptionId { get; init; }

        public DateTime Created { get; init; }

        public DateTime? Handled { get; init; }

        public string ExceptionType { get; init; } = null!;

        public string? Method { get; init; }

        public string StackTrace { get; init; } = null!;

        public string Message { get; init; } = null!;

        public string? Other { get; init; }

        public int? Request { get; init; }
    }
}
