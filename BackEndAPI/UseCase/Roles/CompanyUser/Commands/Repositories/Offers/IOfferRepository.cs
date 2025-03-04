using Domain.Features.Branches.ValueObjects;
using Domain.Features.Companies.ValueObjects;
using Domain.Features.OfferTemplates.ValueObjects;
using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using DomainOffer = Domain.Features.Offers.Entities.Offer;

namespace UseCase.Roles.CompanyUser.Commands.Repositories.Offers
{
    public interface IOfferRepository
    {
        Task CreateAsync(
            IEnumerable<DomainOffer> items,
            CancellationToken cancellationToken);
        Task<HttpCode> IsAllowedOperationAsync(
            PersonId personId,
            CompanyId companyId,
            OfferTemplateId offerTemplateId,
            CancellationToken cancellationToken);
        Task<IEnumerable<Guid>> GetNotFoundedBranchIdsAsync(
            CompanyId companyId,
            IEnumerable<BranchId> branchIds,
            CancellationToken cancellationToken);
    }
}
