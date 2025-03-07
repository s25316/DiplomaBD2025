// Ignore Spelling: Dto

using UseCase.Shared.DTOs.Responses.Companies;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response
{
    public class CompanyAndBranchDto
    {
        public required CompanyDto Company { get; set; }
        public required BranchDto Branch { get; set; }
    }
}
