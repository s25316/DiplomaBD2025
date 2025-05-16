using Domain.Shared.Enums;
using UseCase.Shared.Responses.CommandResults;

namespace UseCase.Shared.Responses.ItemResponse
{
    public class CommandResponse<TCommand> : ItemResponse<BaseCommandResult<TCommand>>
    {
        // Methods
        public static CommandResponse<TCommand> PrepareResponse(
            HttpCode code,
            TCommand command,
            string? message = null)
        {
            var intCode = (int)code;
            return new CommandResponse<TCommand>
            {
                HttpCode = code,
                Result = new BaseCommandResult<TCommand>
                {
                    Item = command,
                    IsCorrect = intCode > 200 && intCode < 300,
                    Message = message ?? code.Description(),
                },
            };
        }
    }
}
