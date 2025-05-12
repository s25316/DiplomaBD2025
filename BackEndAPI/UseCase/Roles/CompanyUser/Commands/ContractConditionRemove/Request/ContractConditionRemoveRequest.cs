using UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Response;
using UseCase.Shared.Requests;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Request
{
    public class ContractConditionRemoveRequest : BaseRequest<ContractConditionRemoveResponse>
    {
        public required Guid ContractConditionId { get; init; }
    }
}
