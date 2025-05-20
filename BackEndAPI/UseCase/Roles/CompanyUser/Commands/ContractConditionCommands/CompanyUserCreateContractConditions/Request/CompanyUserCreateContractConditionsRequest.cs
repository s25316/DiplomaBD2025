using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserCreateContractConditions.Request
{
    public class CompanyUserCreateContractConditionsRequest : BaseRequest<CommandsResponse<CompanyUserCreateContractConditionsCommand>>
    {
        public required Guid CompanyId { get; init; }
        public required IEnumerable<CompanyUserCreateContractConditionsCommand> Commands { get; init; }
    }
}
