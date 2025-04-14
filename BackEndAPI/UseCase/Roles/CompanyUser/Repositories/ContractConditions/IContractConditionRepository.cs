using UseCase.Shared.Templates.Repositories;
using DomainContractCondition = Domain.Features.ContractConditions.Aggregates.ContractCondition;
using DomainContractConditionId = Domain.Features.ContractConditions.ValueObjects.Ids.ContractConditionId;

namespace UseCase.Roles.CompanyUser.Repositories.ContractConditions
{
    public interface IContractConditionRepository
        : IRepositoryTemplate<DomainContractCondition, DomainContractConditionId>
    {
    }
}
