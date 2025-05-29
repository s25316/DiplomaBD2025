using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitmentSetMessage.Request
{
    public class UserRecruitmentSetMessageRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid RecruitmentId { get; init; }
        public required UserRecruitmentSetMessageCommand Command { get; init; }
    }
}
