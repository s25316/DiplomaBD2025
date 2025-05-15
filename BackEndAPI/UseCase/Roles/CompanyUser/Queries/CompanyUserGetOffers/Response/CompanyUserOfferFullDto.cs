using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetOffers.Response
{
    public class CompanyUserOfferFullDto
    {
        public required CompanyDto Company { get; init; }

        public required CompanyUserBranchDto Branch { get; init; }

        public required CompanyUserOfferTemplateDto OfferTemplate { get; init; }

        public required OfferDto Offer { get; init; }

        public required IEnumerable<CompanyUserContractConditionDto> ContractConditions { get; init; }
    }
}
