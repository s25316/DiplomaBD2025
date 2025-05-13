using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.BaseResponses.Guest;

namespace UseCase.Roles.Guests.Queries.GuestGetOffers.Response
{
    public class GuestOfferFullDto
    {
        public required CompanyDto Company { get; init; }

        public required GuestBranchDto Branch { get; init; }

        public required GuestOfferTemplateDto OfferTemplate { get; init; }

        public required OfferDto Offer { get; init; }

        public required IEnumerable<GuestContractConditionDto> ContractConditions { get; init; }
    }
}
