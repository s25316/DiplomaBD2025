using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.Guest;

namespace UseCase.Roles.Guests.Queries.GuestGetContractConditions.Response
{
    public class GuestContractConditionAndCompanyDto
    {
        public required CompanyDto Company { get; init; }

        public required GuestContractConditionDto ContractCondition { get; init; }

        public required int OfferCount { get; init; }
    }
}
