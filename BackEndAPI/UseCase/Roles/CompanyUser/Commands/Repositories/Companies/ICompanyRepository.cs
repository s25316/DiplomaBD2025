using Domain.Features.People.ValueObjects;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.Repositories.Companies
{
    public interface ICompanyRepository
    {
        Task CreateAsync(
            PersonId personId,
            DomainCompany item,
            CancellationToken cancellationToken);
        Task<string> FindDuplicatesAsync(
            DomainCompany domain,
            CancellationToken cancellationToken);
    }
}
