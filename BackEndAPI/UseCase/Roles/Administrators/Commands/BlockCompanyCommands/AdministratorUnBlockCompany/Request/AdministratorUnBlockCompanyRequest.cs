using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.BlockCompanyCommands.AdministratorUnBlockCompany.Request
{
    public class AdministratorUnBlockCompanyRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid CompanyId { get; init; }
    }
}
