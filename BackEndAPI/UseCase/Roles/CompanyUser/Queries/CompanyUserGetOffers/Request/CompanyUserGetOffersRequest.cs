using Domain.Features.Offers.Enums;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOffers.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetOffers.Request
{
    public class CompanyUserGetOffersRequest : GetOfferRequest<ItemsResponse<CompanyUserOfferFullDto>>
    {
        public required OfferStatus? Status { get; init; }
    }
}
