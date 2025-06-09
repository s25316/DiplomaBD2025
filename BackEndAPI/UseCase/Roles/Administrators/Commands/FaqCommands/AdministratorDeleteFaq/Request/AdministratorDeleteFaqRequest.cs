using UseCase.Shared.Requests;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorDeleteFaq.Request
{
    public class AdministratorDeleteFaqRequest : BaseRequest<ResultMetadataResponse>
    {
        public required Guid FaqId { get; init; }
    }
}
