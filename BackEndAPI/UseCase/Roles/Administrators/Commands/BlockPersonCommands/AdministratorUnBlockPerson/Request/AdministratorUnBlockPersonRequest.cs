using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorUnBlockPerson.Request
{
    public class AdministratorUnBlockPersonRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid PersonId { get; init; }
    }
}
