using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserRecruitmentSetMessage.Request
{
    public class CompanyUserRecruitmentSetMessageRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid RecruitmentId { get; init; }
        public required CompanyUserRecruitmentSetMessageCommand Command { get; init; }
    }
}
