using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.BlockCompanyCommands.AdministratorBlockCompany.Request
{
    public class AdministratorBlockCompanyRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid CompanyId { get; init; }
        public required AdministratorBlockCompanyCommand Command { get; init; }
    }
}
