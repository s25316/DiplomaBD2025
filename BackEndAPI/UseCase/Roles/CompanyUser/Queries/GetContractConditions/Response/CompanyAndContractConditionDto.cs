// Ignore Spelling: Dto

using UseCase.Shared.DTOs.Responses.Companies;

namespace UseCase.Roles.CompanyUser.Queries.GetContractConditions.Response
{
    public class CompanyAndContractConditionDto
    {
        public required CompanyDto Company { get; init; }

        public required ContractConditionDto ContractCondition { get; init; }
    }
}
