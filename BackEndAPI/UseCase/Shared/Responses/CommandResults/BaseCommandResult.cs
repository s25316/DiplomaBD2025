namespace UseCase.Shared.Responses.CommandResults
{
    public class BaseCommandResult<T> : ResultMetadata
    {
        public required T Item { get; init; }
    }
}
