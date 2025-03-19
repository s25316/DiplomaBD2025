// Ignore Spelling: Dto

using UseCase.Shared.DTOs.Responses.Companies;

namespace UseCase.Roles.CompanyUser.Queries.GetBranches.Response
{
    public class CompanyAndBranchDto
    {
        public required CompanyDto Company { get; init; }
        public required BranchDto Branch { get; init; }
    }
}
