namespace UseCase.Shared.Templates.Response
{
    public class ResponseItemTemplate<T> : ResponseItemMetaData
    {
        public required T Item { get; init; }
    }
}
