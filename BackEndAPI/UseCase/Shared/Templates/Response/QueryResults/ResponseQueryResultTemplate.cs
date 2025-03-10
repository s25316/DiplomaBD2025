namespace UseCase.Shared.Templates.Response.QueryResults
{
    public class ResponseQueryResultTemplate<T>
    {
        public required IEnumerable<T> Items { get; init; } = [];
        public required int TotalCount { get; init; } = 0;
    }
}
