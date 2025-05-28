using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse.FileResponses;

namespace UseCase.Roles.Users.Queries.GetPersonRecruitmentFile.Request
{
    public class GetPersonRecruitmentFileRequest : BaseRequest<FileResponse>
    {
        public required Guid RecruitmentId { get; init; }
    }
}
