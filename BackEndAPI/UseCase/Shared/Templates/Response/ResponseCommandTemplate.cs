namespace UseCase.Shared.Templates.Response
{
    public class ResponseCommandTemplate<T> : ResponseCommandMetaData
    {
        public required T Item { get; init; }
    }
}
