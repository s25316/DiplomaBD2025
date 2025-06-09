using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.AdministrationCommands.AdministratorGrandAdministrator.Request
{
    public class AdministratorGrandAdministratorRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid PersonId { get; init; }
    }
}
