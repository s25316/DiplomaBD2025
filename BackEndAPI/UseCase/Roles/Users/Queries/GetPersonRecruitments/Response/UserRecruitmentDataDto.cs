using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;
using UseCase.Shared.Responses.BaseResponses.Guest;

namespace UseCase.Roles.Users.Queries.GetPersonRecruitments.Response
{
    public class UserRecruitmentDataDto
    {
        public required RecruitmentDto Recruitment { get; init; }

        public required CompanyUserPersonProfile Person { get; init; }

        public required CompanyDto Company { get; init; }

        public required GuestBranchDto Branch { get; init; }

        public required GuestOfferTemplateDto OfferTemplate { get; init; }

        public required OfferDto Offer { get; init; }

        public required IEnumerable<GuestContractConditionDto> ContractConditions { get; init; }
    }
}
