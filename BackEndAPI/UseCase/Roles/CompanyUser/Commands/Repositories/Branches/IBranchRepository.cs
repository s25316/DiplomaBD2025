using Domain.Features.Companies.ValueObjects;
using Domain.Features.People.ValueObjects;
using DomainBranch = Domain.Features.Branches.Entities.Branch;

namespace UseCase.Roles.CompanyUser.Commands.Repositories.Branches
{
    public interface IBranchRepository
    {
        Task CreateAsync(
            IEnumerable<DomainBranch> items,
            CancellationToken cancellationToken);
        Task<bool> HasAccessToCompany(
            PersonId personId,
            CompanyId companyId,
            CancellationToken cancellationToken);
    }
}
