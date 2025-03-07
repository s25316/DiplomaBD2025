namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response
{
    public class GetCompanyBranchesQueryResult
    {
        public required IEnumerable<CompanyAndBranchDto> Items { get; init; } = [];
        public required int TotalCount { get; init; } = 0;
    }
}
