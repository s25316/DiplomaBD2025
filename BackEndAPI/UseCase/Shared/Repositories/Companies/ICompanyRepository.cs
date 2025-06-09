using UseCase.Shared.Repositories.BaseEFRepository;
using DomainCompany = Domain.Features.Companies.Entities.Company;
using DomainCompanyId = Domain.Features.Companies.ValueObjects.Ids.CompanyId;

namespace UseCase.Shared.Repositories.Companies
{
    public interface ICompanyRepository : IRepositoryTemplate<DomainCompany, DomainCompanyId>
    {
    }
}
