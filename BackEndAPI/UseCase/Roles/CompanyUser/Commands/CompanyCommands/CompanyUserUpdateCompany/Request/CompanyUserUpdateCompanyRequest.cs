using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserUpdateCompany.Request
{
    public class CompanyUserUpdateCompanyRequest : BaseRequest<CommandResponse<CompanyUserUpdateCompanyCommand>>
    {
        public required Guid CompanyId { get; init; }
        public required CompanyUserUpdateCompanyCommand Command { get; init; }
    }
}
