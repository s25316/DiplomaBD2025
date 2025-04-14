using UseCase.Shared.Templates.Repositories;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Aggregates.OfferTemplate;
using DomainOfferTemplateId = Domain.Features.OfferTemplates.ValueObjects.Ids.OfferTemplateId;

namespace UseCase.Roles.CompanyUser.Repositories.OfferTemplates
{
    public interface IOfferTemplateRepository
        : IRepositoryTemplate<DomainOfferTemplate, DomainOfferTemplateId>
    {
    }
}
