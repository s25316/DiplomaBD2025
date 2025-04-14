using UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Request
{
    public class ContractConditionRemoveRequest : RequestTemplate<ContractConditionRemoveResponse>
    {
        public required Guid ContractConditionId { get; init; }
    }
}
