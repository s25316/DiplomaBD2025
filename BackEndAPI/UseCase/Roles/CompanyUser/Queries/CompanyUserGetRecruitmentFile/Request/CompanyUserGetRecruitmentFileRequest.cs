using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse.FileResponses;

namespace UseCase.Roles.CompanyUser.Queries.CompanyUserGetRecruitmentFile.Request
{
    public class CompanyUserGetRecruitmentFileRequest : BaseRequest<FileResponse>
    {
        public required Guid RecruitmentId { get; init; }
    }
}
