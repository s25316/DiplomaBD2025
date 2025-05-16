using UseCase.Shared.Repositories.BaseEFRepository;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Aggregates.OfferTemplate;
using DomainOfferTemplateId = Domain.Features.OfferTemplates.ValueObjects.Ids.OfferTemplateId;

namespace UseCase.Roles.CompanyUser.Repositories.OfferTemplates
{
    public interface IOfferTemplateRepository
        : IRepositoryTemplate<DomainOfferTemplate, DomainOfferTemplateId>
    {
    }
}
