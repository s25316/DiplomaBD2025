using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorBlockPerson.Request
{
    public class AdministratorBlockPersonRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid PersonId { get; init; }
        public required AdministratorBlockPersonCommand Command { get; init; }
    }
}
