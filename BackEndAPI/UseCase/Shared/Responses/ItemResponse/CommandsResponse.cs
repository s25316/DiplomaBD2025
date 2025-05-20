using UseCase.Shared.Responses.CommandResults;

namespace UseCase.Shared.Responses.ItemResponse
{
    public class CommandsResponse<TCommand>
        : ItemResponse<IEnumerable<BaseCommandResult<TCommand>>>
    {
    }
}
