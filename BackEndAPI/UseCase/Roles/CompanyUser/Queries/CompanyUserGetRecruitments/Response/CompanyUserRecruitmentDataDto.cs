// Ignore Spelling: Dto
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetRecruitments.Response
{
    public class CompanyUserRecruitmentDataDto
    {
        public required RecruitmentDto Recruitment { get; init; }

        public required CompanyUserPersonProfile Person { get; init; }

        public required CompanyDto Company { get; init; }

        public required CompanyUserBranchDto Branch { get; init; }

        public required CompanyUserOfferTemplateDto OfferTemplate { get; init; }

        public required OfferDto Offer { get; init; }

        public required IEnumerable<CompanyUserContractConditionDto> ContractConditions { get; init; }
    }
}
