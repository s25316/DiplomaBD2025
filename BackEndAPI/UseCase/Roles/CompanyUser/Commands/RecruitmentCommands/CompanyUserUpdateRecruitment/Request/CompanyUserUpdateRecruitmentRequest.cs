using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserUpdateRecruitment.Request
{
    public class CompanyUserUpdateRecruitmentRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid RecruitmentId { get; init; }
        public required CompanyUserUpdateRecruitmentCommand Command { get; init; }
    }
}
