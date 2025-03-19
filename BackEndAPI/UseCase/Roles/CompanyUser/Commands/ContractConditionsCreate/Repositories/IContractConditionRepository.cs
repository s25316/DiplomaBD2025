using UseCase.Shared.Templates.Repositories;
using DomainContractCondition = Domain.Features.ContractConditions.Entities.ContractCondition;

namespace UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Repositories
{
    public interface IContractConditionRepository : IRepositoryTemplate<DomainContractCondition>
    {
    }
}
