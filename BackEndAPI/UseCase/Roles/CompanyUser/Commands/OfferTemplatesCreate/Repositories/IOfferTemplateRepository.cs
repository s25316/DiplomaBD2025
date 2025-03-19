using UseCase.Shared.Templates.Repositories;
using DomainOfferTemplate = Domain.Features.OfferTemplates.Entities.OfferTemplate;

namespace UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Repositories
{
    public interface IOfferTemplateRepository : IRepositoryTemplate<DomainOfferTemplate>
    {
    }
}
