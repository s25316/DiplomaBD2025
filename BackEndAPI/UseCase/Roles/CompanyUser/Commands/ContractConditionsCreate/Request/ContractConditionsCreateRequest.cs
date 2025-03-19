using UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Request
{
    public class ContractConditionsCreateRequest : RequestTemplate<ContractConditionsCreateResponse>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<ContractConditionsCreateCommand> Commands { get; init; }
    }
}
