using UseCase.Shared.Templates.Repositories;
using DomainOffer = Domain.Features.Offers.Entities.Offer;

namespace UseCase.Roles.CompanyUser.Commands.OffersCreate.Repositories
{
    public interface IOfferRepository : IRepositoryTemplate<DomainOffer>
    {
    }
}
