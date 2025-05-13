using UseCase.Roles.Guests.Queries.GuestGetOffers.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Guests.Queries.GuestGetOffers.Request
{
    public class GuestGetOffersRequest : GetOfferRequest<ItemsResponse<GuestOfferFullDto>>
    {
    }
}
