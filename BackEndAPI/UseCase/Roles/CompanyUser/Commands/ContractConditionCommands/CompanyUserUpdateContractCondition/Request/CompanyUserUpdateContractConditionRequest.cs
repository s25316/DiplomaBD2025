using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserUpdateContractCondition.Request
{
    public class CompanyUserUpdateContractConditionRequest : BaseRequest<CommandResponse<CompanyUserUpdateContractConditionCommand>>
    {
        public required Guid ContractConditionId { get; init; }
        public required CompanyUserUpdateContractConditionCommand Command { get; init; }
    }
}
