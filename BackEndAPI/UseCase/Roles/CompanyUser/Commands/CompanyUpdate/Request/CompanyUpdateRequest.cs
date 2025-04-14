using UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Request
{
    public class CompanyUpdateRequest : RequestTemplate<CompanyUpdateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required CompanyUpdateCommand Command { get; init; }
    }
}
