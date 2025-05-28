using UseCase.Roles.CompanyUser.Queries.CompanyUserGetRecruitments.Response;
using UseCase.Shared.Requests.BaseRequests;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetRecruitments.Request
{
    public class CompanyUserGetRecruitmentsRequest : GetRecruitmentsRequest<ItemsResponse<CompanyUserRecruitmentDataDto>>
    {
        // Company Parameters
        public required Guid? CompanyId { get; init; }

        // Branch Parameters
        public required Guid? BranchId { get; init; }

        // Offer Parameters
        public required Guid? OfferId { get; init; }

        // Person Parameters
        public required string? PersonEmail { get; init; }
        public required string? PersonPhoneNumber { get; init; }
    }
}
