// Ignore Spelling: Dto

using UseCase.Shared.Responses.BaseResponses;
using UseCase.Shared.Responses.BaseResponses.CompanyUser;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches.Response
{
    public class CompanyUserGetBranchAndCompanyDto
    {
        public required CompanyDto Company { get; init; }
        public required CompanyUserBranchDto Branch { get; init; }
    }
}
