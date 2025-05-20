using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserRemoveContractCondition.Request
{
    public class CompanyUserRemoveContractConditionRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid ContractConditionId { get; init; }
    }
}
