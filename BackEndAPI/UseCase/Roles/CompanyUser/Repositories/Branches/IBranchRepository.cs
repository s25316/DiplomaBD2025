using UseCase.Shared.Repositories.BaseEFRepository;
using DomainBranch = Domain.Features.Branches.Entities.Branch;
using DomainBranchId = Domain.Features.Branches.ValueObjects.BranchId;

namespace UseCase.Roles.CompanyUser.Repositories.Branches
{
    public interface IBranchRepository : IRepositoryTemplate<DomainBranch, DomainBranchId>
    {
    }
}
