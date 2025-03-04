using UseCase.Shared.DTOs.Responses.Companies;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response
{
    public class GetCompanyBranchesResponse
    {
        public required IEnumerable<BranchDto> Branches { get; init; } = [];
    }
}
