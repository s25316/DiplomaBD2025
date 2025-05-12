using UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Request
{
    public class ContractConditionsCreateRequest : BaseRequest<ContractConditionsCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<ContractConditionsCreateCommand> Commands { get; init; }
    }
}
