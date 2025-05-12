namespace UseCase.Shared.Responses.ItemsResponse
{
    public class ItemsResult<TItem>
    {
        public required IEnumerable<TItem> Items { get; init; }
        public required int TotalCount { get; init; }
    }
}
