namespace UseCase.Shared.Templates.Response.Commands
{
    public class ResponseCommandTemplate<T> : ResponseCommandMetadata
    {
        public required T Item { get; init; }
    }
}
