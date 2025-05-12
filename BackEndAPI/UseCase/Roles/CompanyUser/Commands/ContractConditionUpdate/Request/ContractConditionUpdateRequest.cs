using UseCase.Roles.CompanyUser.Commands.ContractConditionUpdate.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionUpdate.Request
{
    public class ContractConditionUpdateRequest : BaseRequest<ContractConditionUpdateResponse>
    {
        public required Guid ContractConditionId { get; init; }
        public required ContractConditionsUpdateCommand Command { get; init; }
    }
}
