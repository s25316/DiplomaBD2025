using UseCase.Shared.Templates.Repositories;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Repositories
{
    public interface ICompanyRepository : IRepositoryTemplate<DomainCompany>
    {
    }
}
