using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.Guest;

namespace UseCase.Roles.Guests.Queries.GuestGetOfferTemplates.Response
{
    public class GuestOfferTemplateAndCompanyDto
    {
        public required CompanyDto Company { get; init; }

        public required GuestOfferTemplateDto OfferTemplate { get; init; }

        public required int OfferCount { get; init; }
    }
}
