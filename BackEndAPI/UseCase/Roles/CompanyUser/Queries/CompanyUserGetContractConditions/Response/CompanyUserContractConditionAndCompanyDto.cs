// Ignore Spelling: Dto
using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions.Response
{
    public class CompanyUserContractConditionAndCompanyDto
    {
        public required CompanyDto Company { get; init; }

        public required CompanyUserContractConditionDto ContractCondition { get; init; }

        public required int OfferCount { get; init; }
    }
}
