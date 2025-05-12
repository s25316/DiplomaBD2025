using UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Request
{
    public class CompanyUpdateRequest : BaseRequest<CompanyUpdateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required CompanyUpdateCommand Command { get; init; }
    }
}
