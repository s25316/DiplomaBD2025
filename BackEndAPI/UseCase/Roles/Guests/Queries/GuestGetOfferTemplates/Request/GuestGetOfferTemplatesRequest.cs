using UseCase.Roles.Guests.Queries.GuestGetOfferTemplates.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Guests.Queries.GuestGetOfferTemplates.Request
{
    public class GuestGetOfferTemplatesRequest : GetOfferTemplatesRequest<ItemsResponse<GuestOfferTemplateAndCompanyDto>>
    {
        // Sorting
        public required GuestOfferTemplateOrderBy OrderBy { get; init; }
    }
}
