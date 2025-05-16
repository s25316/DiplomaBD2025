using UseCase.Shared.Repositories.BaseEFRepository;
using DomainOffer = Domain.Features.Offers.Aggregates.Offer;
using DomainOfferId = Domain.Features.Offers.ValueObjects.Ids.OfferId;

namespace UseCase.Roles.CompanyUser.Repositories.Offers
{
    public interface IOfferRepository : IRepositoryTemplate<DomainOffer, DomainOfferId>
    {
    }
}
