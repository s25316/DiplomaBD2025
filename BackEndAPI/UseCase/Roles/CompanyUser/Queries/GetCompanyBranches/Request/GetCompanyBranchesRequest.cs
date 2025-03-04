using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Response;
using UseCase.Shared.Templates.Requests;

namespace UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Request
{
    public class GetCompanyBranchesRequest : RequestTemplate<GetCompanyBranchesResponse>
    {
        public required Guid CompanyId { get; init; }
    }
}
