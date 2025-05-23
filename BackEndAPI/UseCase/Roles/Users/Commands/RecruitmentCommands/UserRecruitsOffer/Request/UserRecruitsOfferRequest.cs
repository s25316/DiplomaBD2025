using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitsOffer.Request
{
    public class UserRecruitsOfferRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid OfferId { get; init; }

        public required UserRecruitsOfferCommand Command { get; init; }
    }
}
