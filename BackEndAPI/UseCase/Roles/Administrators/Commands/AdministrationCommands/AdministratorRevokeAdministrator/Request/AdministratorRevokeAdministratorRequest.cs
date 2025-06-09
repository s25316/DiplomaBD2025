using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.AdministrationCommands.AdministratorRevokeAdministrator.Request
{
    public class AdministratorRevokeAdministratorRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid PersonId { get; init; }
    }
}
