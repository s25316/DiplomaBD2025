using UseCase.Shared.Templates.Repositories;
using DomainBranch = Domain.Features.Branches.Entities.Branch;

namespace UseCase.Roles.CompanyUser.Commands.BranchesCreate.Repositories
{
    public interface IBranchRepository : IRepositoryTemplate<DomainBranch>
    {
    }
}
