using Domain.Features.Companies.ValueObjects;
using Domain.Features.People.ValueObjects;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Entities.OfferTemplate;

namespace UseCase.Roles.CompanyUser.Commands.Repositories.OfferTemplates
{
    public interface IOfferTemplateRepository
    {
        Task<bool> HasAccessToCompany(
            PersonId personId,
            CompanyId companyId,
            CancellationToken cancellationToken);
        Task CreateAsync(IEnumerable<DomainOfferTemplate> items, CancellationToken cancellationToken);
    }
}
